import { ErrorService } from './error.service';
import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpResponse } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { ICampaign, INewCampaign } from './../models/campaign';
import { Endpoints } from '../constants';

@Injectable({
    providedIn: 'root'
})
export class CampaignService {
    constructor(
        private http: HttpClient,
        private ErrorService: ErrorService
    ) { }

    public getAll(): Observable<ICampaign[]> {
        return this.http.get<ICampaign[]>(`${Endpoints.campaigns}`)
            .pipe(
                catchError(this.errorHandler.bind(this))
            )
    }

    public getByNonProfitId(): Observable<ICampaign[]> {
        return this.http.get<ICampaign[]>(`${Endpoints.npCampaigns}`)
            .pipe(
                catchError(this.errorHandler.bind(this))
            )
    }

    public create(body: INewCampaign): Observable<HttpResponse<number>> {
        return this.http.post<number>(`${Endpoints.campaigns}`, body, { observe: 'response' })
            .pipe(
                catchError(this.errorHandler.bind(this))
            )
    }

    private errorHandler(error: HttpErrorResponse) {
        this.ErrorService.handle(error.message)
        return throwError(() => error.message)
    }

}
