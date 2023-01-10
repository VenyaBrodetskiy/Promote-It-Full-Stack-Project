import { ErrorService } from './error.service';
import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { ICampaign } from './../models/campaign';


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

    const headers = new HttpHeaders().set('Authorization', 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyRGF0YSI6eyJ1c2VySWQiOjQsInVzZXJUeXBlSWQiOjF9LCJpYXQiOjE2NzMzNTgwNzYsImV4cCI6MTY3MzM2NTI3Nn0.RRCKHngHNrjcmKDCYJeBXPxc9-Uu7ROQuwxF_s73lQs');

    return this.http.get<ICampaign[]>('http://localhost:6060/api/campaign/', { headers })
      .pipe(
        catchError(this.errorHandler.bind(this))
    )
  }

  private errorHandler(error: HttpErrorResponse) {
    this.ErrorService.handle(error.message)
    return throwError(() => error.message)
  }

}
