import { Endpoints } from 'src/app/constants';
import { Observable, catchError, throwError } from 'rxjs';
import { IBusinessOwner, ISocialActivist, INonProfitOrg } from './../models/user';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ErrorService } from './error.service';

@Injectable({
    providedIn: 'root'
})
export class UserService {

    constructor(
        private http: HttpClient,
        private errorService: ErrorService
    ) { }

    public addBusinessOwner(user: IBusinessOwner): Observable<HttpResponse<number>> {
        return this.addUser<IBusinessOwner>(user, Endpoints.addBusinessOwner);
    }

    public addSocialActivist(user: ISocialActivist): Observable<HttpResponse<number>> {
        return this.addUser<ISocialActivist>(user, Endpoints.addSocialActivist);
    }

    public addNonProfitUser(user: INonProfitOrg): Observable<HttpResponse<number>> {
        return this.addUser<INonProfitOrg>(user, Endpoints.addNonProfitUser);
    }

    private addUser<T>(user: T, endpoint: string): Observable<HttpResponse<number>> {

        return this.http.post<number>(endpoint, user, { observe: "response" })
            .pipe(
                catchError(this.errorHandler.bind(this))
            );
    }

    private errorHandler(error: HttpErrorResponse) {
        this.errorService.handle(error.message)
        return throwError(() => error.message)
    }

}
