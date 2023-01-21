import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { Endpoints } from '../constants';
import { ITransaction } from '../models/transaction';
import { ErrorService } from './error.service';
import { NGXLogger } from 'ngx-logger';

@Injectable({
    providedIn: 'root'
})
export class TransactionService {

    constructor(
        private http: HttpClient,
        private errorService: ErrorService,
        private logger: NGXLogger
    ) { }

    public create(body: ITransaction): Observable<HttpResponse<ITransaction>> {
        this.logger.info(`Creating new transaction: ${body}`);
        return this.http.post<ITransaction>(`${Endpoints.createTransaction}`, body, { observe: 'response' })
            .pipe(
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
            );
    }
}
