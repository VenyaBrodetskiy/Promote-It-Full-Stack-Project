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

  getAll(): Observable<ICampaign[]> {

    const headers = new HttpHeaders().set('Authorization', 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyRGF0YSI6eyJ1c2VySWQiOjQsInVzZXJUeXBlSWQiOjF9LCJpYXQiOjE2NzM1NTg3NDYsImV4cCI6MTY3MzU2NTk0Nn0.v_PWVfmoAcaS6nvCdQIHUWJyfh6KiVdoU5CETU5nTDc');

    return this.http.get<ICampaign[]>(`${Endpoints.campaigns}`, { headers })
      .pipe(
        catchError(this.errorHandler.bind(this))
    )
  }

  create(body: INewCampaign): Observable<number> {

    const headers = new HttpHeaders().set('Authorization', 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyRGF0YSI6eyJ1c2VySWQiOjQsInVzZXJUeXBlSWQiOjF9LCJpYXQiOjE2NzM1NTE1MTUsImV4cCI6MTY3MzU1ODcxNX0.Ug1vFA8YWFKEQN5aMkzecZVT0Is6IHvFa5_sdpzbxHs');

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
