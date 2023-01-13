import { ErrorService } from './error.service';
import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
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
  ) {
  }

  public getAll(): Observable<ICampaign[]> {

    //const headers = new HttpHeaders().set('Authorization', 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyRGF0YSI6eyJ1c2VySWQiOjQsInVzZXJUeXBlSWQiOjF9LCJpYXQiOjE2NzM2Mzk4NTEsImV4cCI6MTY3MzY0NzA1MX0.OWRBW13dO440A8YqfwvSOQ-BCvGxXxAl_PEKfJD5OEk');

    return this.http.get<ICampaign[]>(`${Endpoints.campaigns}`)
      .pipe(
        catchError(this.errorHandler.bind(this))
    )
  }

  //TODO: remove? for social activist
  public create(body: INewCampaign): Observable<number> {

    const headers = new HttpHeaders().set('Authorization', 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyRGF0YSI6eyJ1c2VySWQiOjQsInVzZXJUeXBlSWQiOjF9LCJpYXQiOjE2NzM2Mzk4NTEsImV4cCI6MTY3MzY0NzA1MX0.OWRBW13dO440A8YqfwvSOQ-BCvGxXxAl_PEKfJD5OEk');

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
