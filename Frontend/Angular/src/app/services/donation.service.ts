import { HttpClient, HttpResponse } from '@angular/common/http';
import { ErrorService } from './error.service';
import { Injectable } from '@angular/core';
import { IDonation } from '../models/donation';
import { catchError, Observable, throwError } from 'rxjs';
import { Endpoints } from '../constants';
import { NGXLogger } from 'ngx-logger';

@Injectable({
    providedIn: 'root'
})
export class DonationService {

    constructor(
        private http: HttpClient,
        private errorService: ErrorService,
        private logger: NGXLogger
    ) { }

    public create(body: IDonation): Observable<HttpResponse<number>> {
        this.logger.info(`Donating product to campaign: `, body);
        return this.http.post<number>(`${Endpoints.donateProductToCampaign}`, body, { observe: 'response' })
            .pipe(
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
            )
    }
}
