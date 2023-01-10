import { ErrorService } from './error.service';
import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { ICampaign } from './../models/campaign';
import { Endpoints } from '../constants';


@Injectable({
  providedIn: 'root'
})

export class CampaignService {
  constructor(
    private http: HttpClient,
    private ErrorService: ErrorService

  ) {
  }

  getAll(): Observable<ICampaign[]> {

    const headers = new HttpHeaders().set('Authorization', 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyRGF0YSI6eyJ1c2VySWQiOjQsInVzZXJUeXBlSWQiOjF9LCJpYXQiOjE2NzMzNjUzNDgsImV4cCI6MTY3MzM3MjU0OH0.dkG5FN35u7oHPtjePJzPB1leLDnmPwTs3629ng_o2pw');

    return this.http.get<ICampaign[]>(`${Endpoints.campaigns}`, { headers })
      .pipe(
        catchError(this.errorHandler.bind(this))
    )
  }

  create(body: ICampaign): Observable<number> {

    const headers = new HttpHeaders().set('Authorization', 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyRGF0YSI6eyJ1c2VySWQiOjYsInVzZXJUeXBlSWQiOjN9LCJpYXQiOjE2NzMzNjY5OTgsImV4cCI6MTY3MzM3NDE5OH0.KfX0svVOaHKG3RGc1f1vseRgjHztSv4sDK7Yi6J2WAs');

    return this.http.post<number>('http://localhost:6060/api/campaign/', body, { headers })
      .pipe(
        catchError(this.errorHandler.bind(this))
      )
  }


  private errorHandler(error: HttpErrorResponse) {
    this.ErrorService.handle(error.message)
    return throwError(() => error.message)
  }

}
