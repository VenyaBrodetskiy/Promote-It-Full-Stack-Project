import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { Endpoints } from '../constants';
import { IBalance } from '../models/balance';
import { ErrorService } from './error.service';
import { NGXLogger } from 'ngx-logger';

@Injectable({
    providedIn: 'root'
})
export class BalanceService {

    constructor(
        private http: HttpClient,
        private errorService: ErrorService,
        private logger: NGXLogger
    ) { }

    public getBalanceByCampaignId(campaignId: number): Observable<IBalance[]> {
        this.logger.info(`Getting balance for campaign id: ${campaignId}`);
        return this.http.get<IBalance[]>(`${Endpoints.getBalanceByCampaignId}${campaignId}`)
            .pipe(
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
            );
    }
}
