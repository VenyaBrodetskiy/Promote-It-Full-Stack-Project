import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from "rxjs";
import { IProduct } from '../models/product';
import { ErrorService } from './error.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(
    private http: HttpClient,
    private ErrorService: ErrorService
  ) { }

  create(body: IProduct): Observable<number> {

    return this.http.post<number>('https://localhost:7121/api/BusinessOwner/AddProduct', body)
      .pipe(
        catchError(this.errorHandler.bind(this))
      )
  }

  private errorHandler(error: HttpErrorResponse) {
    this.ErrorService.handle(error.message)
    return throwError(() => error.message)
  }

}
