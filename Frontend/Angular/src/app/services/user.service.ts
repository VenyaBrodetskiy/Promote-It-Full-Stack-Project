import { Endpoints } from 'src/app/constants';
import { Observable, catchError, throwError } from 'rxjs';
import { IBusinessOwner, ISocialActivist, INonProfitOrg } from './../models/user';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ErrorService } from './error.service';
import { NGXLogger } from 'ngx-logger';

@Injectable({
    providedIn: 'root'
})
export class UserService {

    constructor(
        private http: HttpClient,
        private errorService: ErrorService,
        private logger: NGXLogger
    ) { }

    public addBusinessOwner(user: IBusinessOwner): Observable<HttpResponse<number>> {
        this.logger.info(`Adding new business owner: ${user}`);
        return this.addUser<IBusinessOwner>(user, Endpoints.addBusinessOwner);
    }

    public addSocialActivist(user: ISocialActivist): Observable<HttpResponse<number>> {
        this.logger.info(`Adding new social activist: ${user}`);
        return this.addUser<ISocialActivist>(user, Endpoints.addSocialActivist);
    }

    public addNonProfitUser(user: INonProfitOrg): Observable<HttpResponse<number>> {
        this.logger.info(`Adding new non profit organization: ${user}`);
        return this.addUser<INonProfitOrg>(user, Endpoints.addNonProfitUser);
    }

    private addUser<T>(user: T, endpoint: string): Observable<HttpResponse<number>> {
        return this.http.post<number>(endpoint, user, { observe: "response" })
            .pipe(
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
            );
    }
}
