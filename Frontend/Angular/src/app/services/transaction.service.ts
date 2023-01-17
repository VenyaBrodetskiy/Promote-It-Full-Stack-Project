import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { Endpoints } from '../constants';
import { ITransaction } from '../models/transaction';
import { ErrorService } from './error.service';

@Injectable({
    providedIn: 'root'
})
export class TransactionService {

    constructor(
        private http: HttpClient,
        private errorService: ErrorService
    ) { }

    public create(body: ITransaction): Observable<HttpResponse<ITransaction>> {
        return this.http.post<ITransaction>(`${Endpoints.createTransaction}`, body, { observe: 'response' })
            .pipe(
                catchError(this.errorHandler.bind(this))
            );
    }

    private errorHandler(error: HttpErrorResponse) {
        this.errorService.handle(error.message);
        return throwError(() => error.message);
    }
}
