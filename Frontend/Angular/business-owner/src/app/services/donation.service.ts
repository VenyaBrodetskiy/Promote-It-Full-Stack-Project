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

  create(body: IDonation): Observable<number> {

    const headers = new HttpHeaders().set('Authorization', 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyRGF0YSI6eyJ1c2VySWQiOjYsInVzZXJUeXBlSWQiOjN9LCJpYXQiOjE2NzMzNjY5OTgsImV4cCI6MTY3MzM3NDE5OH0.KfX0svVOaHKG3RGc1f1vseRgjHztSv4sDK7Yi6J2WAs');

    return this.http.post<number>(`${Endpoints.donateProductToCampaign}`, body, { headers })
      .pipe(
        catchError(this.errorHandler.bind(this))
      )
  }

  private errorHandler(error: HttpErrorResponse) {
    this.ErrorService.handle(error.message)
    return throwError(() => error.message)
  }

}
