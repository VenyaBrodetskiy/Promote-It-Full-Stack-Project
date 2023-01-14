import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { ErrorService } from './error.service';
import { Injectable } from '@angular/core';
import { IDonation } from '../models/donation';
import { catchError, Observable, throwError } from 'rxjs';
import { Endpoints } from '../constants';

@Injectable({
    providedIn: 'root'
})
export class DonationService {

    constructor(
        private http: HttpClient,
        private ErrorService: ErrorService
    ) { }

    public create(body: IDonation): Observable<number> {

        return this.http.post<number>(`${Endpoints.donateProductToCampaign}`, body)
            .pipe(
                catchError(this.errorHandler.bind(this))
            )
    }

    private errorHandler(error: HttpErrorResponse) {
        this.ErrorService.handle(error.message)
        return throwError(() => error.message)
    }

}
