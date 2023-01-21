import { IProductForCampaignRow } from './../models/table-line';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from "rxjs";
import { Endpoints } from '../constants';
import { INewProduct, IProduct } from '../models/product';
import { ErrorService } from './error.service';
import { NGXLogger } from 'ngx-logger';

@Injectable({
    providedIn: 'root'
})
export class ProductService {

    constructor(
        private http: HttpClient,
        private errorService: ErrorService,
        private logger: NGXLogger
    ) { }

    public getAll(): Observable<IProduct[]> {
        this.logger.info(`Getting all products`);
        return this.http.get<IProduct[]>(`${Endpoints.products}`)
            .pipe(
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
            );
    }

    public getProductsForCampaign(campaignId: number): Observable<IProductForCampaignRow[]> {
        this.logger.info(`Getting all products for campaignId: ${campaignId}`);
        return this.http.get<IProductForCampaignRow[]>(`${Endpoints.getProductsForCampaign}${campaignId}`)
            .pipe(
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
            );
    }

    public create(body: INewProduct): Observable<HttpResponse<number>> {
        this.logger.info(`Creating new product: ${body}`);
        return this.http.post<number>(`${Endpoints.donateNewProduct}`, body, { observe: 'response' })
            .pipe(
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
            );
    }
}
