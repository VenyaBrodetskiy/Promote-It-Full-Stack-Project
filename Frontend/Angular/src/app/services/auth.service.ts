import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable, tap } from "rxjs";
import { Endpoints } from "src/app/constants";
import { HttpClient, HttpHeaders, HttpResponse } from "@angular/common/http";
import { IUser } from '../models/user';
import { ErrorService } from './error.service';

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

    private _userTypeId = new BehaviorSubject<string>('');
    public userTypeId$ = this._userTypeId.asObservable();
    private token: string;
    private userTypeId: string;
    private _isLoggedIn = new BehaviorSubject<boolean>(false);
    public isLoggedIn$ = this._isLoggedIn.asObservable();

    constructor(
        private http: HttpClient,
        private errorService: ErrorService
    ) { }

    public login(user: IUser): Observable<HttpResponse<ILoginResponse>> {
        return this.http.post<ILoginResponse>(Endpoints.login, this.toServerUser(user), { observe: 'response' })
            .pipe(
                map((result: HttpResponse<ILoginResponse>) => {
                    this.token = result.body!.token;
                    localStorage.setItem('token', this.token);
                    this._isLoggedIn.next(true);
                    return result;
                })
            );
    }

    public getUserType(): Observable<HttpResponse<IUserTypeResponse>> {
        return this.http.post<IUserTypeResponse>(Endpoints.userTypeId, null, { observe: 'response' })
            .pipe(
                map((result: HttpResponse<IUserTypeResponse>) => {
                    this.userTypeId = result.body!.userTypeId;
                    this._userTypeId.next(this.userTypeId);
                    localStorage.setItem('userTypeId', this.userTypeId);
                    return result;
                })
            );
    }

    public getHeaders(): HttpHeaders {
        return new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('token')}`);
    }

    public logout() {
        localStorage.clear();
        this._isLoggedIn.next(false);
        this._userTypeId.next('');
    }

    private toServerUser(user: IUser): ILoginRequest {
        return {
            login: user.username,
            password: user.password
        }
    }

}
