import { ErrorService } from './error.service';
import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { Endpoints } from '../constants';
import { IOrder } from '../models/order';

@Injectable({
  providedIn: 'root'
})

export class OrderService {

  constructor(
    private http: HttpClient,
    private ErrorService: ErrorService
  ) { }

  getAll(): Observable<IOrder[]> {

    //TODO: remove hardcoded businessOwnerId=4
    return this.http.get<IOrder[]>(`${Endpoints.orders}?businessOwnerId=4`)
      .pipe(
        catchError(this.errorHandler.bind(this))
      )
  }

  changeState(body: number): Observable<IOrder> {

    //TODO: remove hardcoded businessOwnerId=4
    return this.http.put<IOrder>(`${Endpoints.changeState}?businessOwnerId=4&transactionId=${body}`, null)
      .pipe(
        catchError(this.errorHandler.bind(this))
      )
  }
  


  private errorHandler(error: HttpErrorResponse) {
    this.ErrorService.handle(error.message)
    return throwError(() => error.message)
  }

}
