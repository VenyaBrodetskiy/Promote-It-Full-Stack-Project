import { ErrorService } from './error.service';
import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { Endpoints } from '../constants';
import { IChangeOrder, IOrder } from '../models/order';

@Injectable({
    providedIn: 'root'
})
export class OrderService {

    constructor(
        private http: HttpClient,
        private ErrorService: ErrorService
    ) { }

    public getAll(): Observable<IOrder[]> {

        return this.http.get<IOrder[]>(`${Endpoints.orders}`)
            .pipe(
                catchError(this.errorHandler.bind(this))
            )
    }

    public changeState(body: IChangeOrder): Observable<IOrder> {

        return this.http.put<IOrder>(`${Endpoints.changeState}`, body)
            .pipe(
                catchError(this.errorHandler.bind(this))
            )
    }

    private errorHandler(error: HttpErrorResponse) {
        this.ErrorService.handle(error.message)
        return throwError(() => error.message)
    }

}
