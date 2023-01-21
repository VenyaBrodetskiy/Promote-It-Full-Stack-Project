import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, shareReplay, Subject, takeUntil, tap, throwError } from "rxjs";
import { Endpoints } from "src/app/constants";
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse } from "@angular/common/http";
import { IUser } from '../models/user';
import { ErrorService } from './error.service';
import { NGXLogger } from 'ngx-logger';

interface ILoginRequest {
    login: string;
    password: string;
}

interface ILoginResponse {
    token: string;
}

interface IUserTypeResponse {
    userTypeId: string;
}

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    private _isLoggedIn = new BehaviorSubject<boolean>(false);
    public isLoggedIn$ = this._isLoggedIn.asObservable();

    private _userTypeId = new BehaviorSubject<string>('');
    public userTypeId$ = this._userTypeId.asObservable();

    private token: string;
    private userTypeId: string;

    constructor(
        private http: HttpClient,
        private errorService: ErrorService,
        private logger: NGXLogger
    ) { }

    public login(user: IUser): Observable<HttpResponse<ILoginResponse>> {
        this.logger.info(`Verifying ${user.login} and ${user.password}`);
        return this.http.post<ILoginResponse>(Endpoints.login, this.toServerUser(user), { observe: 'response' })
            .pipe(
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
                map((result: HttpResponse<ILoginResponse>) => {
                    this.token = result.body!.token;
                    localStorage.setItem('token', this.token);
                    localStorage.setItem('isLoggedIn', 'true');
                    this.logger.info(`Token is saved to local Storage`);
                    this._isLoggedIn.next(true);
                    return result;
                })
            );
    }

    public getUserType(): Observable<HttpResponse<IUserTypeResponse>> {
        this.logger.info(`Getting user type by token`);
        return this.http.post<IUserTypeResponse>(Endpoints.userTypeId, null, { observe: 'response' })
            .pipe(
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
                map((result: HttpResponse<IUserTypeResponse>) => {
                    this.userTypeId = result.body!.userTypeId;
                    this._userTypeId.next(this.userTypeId);
                    localStorage.setItem('userTypeId', this.userTypeId);
                    return result;
                })
            );
    }

    public getHeaders(): HttpHeaders {
        this.logger.info(`Setting token for Authorization header`);
        return new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('token')}`);
    }

    public logout() {
        this.logger.info(`Logging out`);
        localStorage.clear();
        this._isLoggedIn.next(false);
        this._userTypeId.next('');
    }

    private toServerUser(user: IUser): ILoginRequest {
        return {
            login: user.login,
            password: user.password
        }
    }
}
