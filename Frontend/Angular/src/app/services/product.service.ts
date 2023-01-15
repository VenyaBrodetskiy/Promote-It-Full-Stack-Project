import { HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from "rxjs";
import { Endpoints } from '../constants';
import { INewProduct, IProduct } from '../models/product';
import { ErrorService } from './error.service';


@Injectable({
    providedIn: 'root'
})
export class ProductService {

    constructor(
        private http: HttpClient,
        private ErrorService: ErrorService
    ) { }

    public getAll(): Observable<IProduct[]> {

        return this.http.get<IProduct[]>(`${Endpoints.products}`)
            .pipe(
                catchError(this.errorHandler.bind(this))
            )
    }

    public create(body: INewProduct): Observable<HttpResponse<number>> {
        return this.http.post<number>(`${Endpoints.donateNewProduct}`, body, { observe: 'response' })
            .pipe(
                catchError(this.errorHandler.bind(this))
            )
    }

    private errorHandler(error: HttpErrorResponse) {
        this.ErrorService.handle(error.message)
        return throwError(() => error.message)
    }

}
