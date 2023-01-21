import { ErrorService } from './error.service';
import { Injectable } from "@angular/core";
import { HttpClient, HttpResponse } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { ICampaign, INewCampaign } from './../models/campaign';
import { Endpoints } from '../constants';
import { NGXLogger } from 'ngx-logger';

@Injectable({
    providedIn: 'root'
})
export class CampaignService {
    constructor(
        private http: HttpClient,
        private errorService: ErrorService,
        private logger: NGXLogger
    ) { }

    public getAll(): Observable<ICampaign[]> {
        this.logger.info(`Getting all campaigns`);
        return this.http.get<ICampaign[]>(`${Endpoints.campaigns}`)
            .pipe(
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
            )
    }

    public getByNonProfitId(): Observable<ICampaign[]> {
        this.logger.info(`Getting all campaigns by non profit user ID`);
        return this.http.get<ICampaign[]>(`${Endpoints.npCampaigns}`)
            .pipe(
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
            )
    }

    public create(body: INewCampaign): Observable<HttpResponse<number>> {
        this.logger.info(`Creating new campaign: ${body}`);
        return this.http.post<number>(`${Endpoints.campaigns}`, body, { observe: 'response' })
            .pipe(
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
            )
    }
}
