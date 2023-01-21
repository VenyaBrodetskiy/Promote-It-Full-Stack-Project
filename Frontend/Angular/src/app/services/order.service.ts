import { ErrorService } from './error.service';
import { Injectable } from "@angular/core";
import { HttpClient, HttpResponse } from "@angular/common/http";
import { catchError, map, Observable, throwError } from "rxjs";
import { Endpoints } from '../constants';
import { IChangeOrder, IOrder } from '../models/order';
import { NGXLogger } from 'ngx-logger';

@Injectable({
    providedIn: 'root'
})
export class OrderService {

    constructor(
        private http: HttpClient,
        private errorService: ErrorService,
        private logger: NGXLogger
    ) { }

    public getAll(): Observable<IOrder[]> {
        this.logger.info(`Getting all orders`);
        return this.http.get<IOrder[]>(`${Endpoints.orders}`)
            .pipe(
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
            )
    }

    public changeState(body: IChangeOrder): Observable < HttpResponse < IOrder >> {
        this.logger.info(`Changing state of the order: `, body);
        return this.http.put<IOrder>(`${Endpoints.changeState}`, body)
            .pipe(
                map(data => new HttpResponse({ body: data })),
                catchError(error => {
                    this.errorService.handle(error.message);
                    return throwError(() => error);
                }),
            );
    }
}
