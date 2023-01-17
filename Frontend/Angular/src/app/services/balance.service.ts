import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { Endpoints } from '../constants';
import { IBalance } from '../models/balance';
import { ErrorService } from './error.service';

@Injectable({
    providedIn: 'root'
})
export class BalanceService {

    constructor(
        private http: HttpClient,
        private errorService: ErrorService
    ) { }

    public getBalanceByCampaignId(campaignId: number): Observable<IBalance[]> {
        return this.http.get<IBalance[]>(`${Endpoints.getBalanceByCampaignId}${campaignId}`)
            .pipe(
                catchError(this.errorHandler.bind(this))
            );
    }

    private errorHandler(error: HttpErrorResponse) {
        this.errorService.handle(error.message);
        return throwError(() => error.message);
    }
}
