import { Injectable } from '@angular/core';
import { map, Observable } from "rxjs";
import { Endpoints } from "src/app/constants";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { IUser } from '../models/user';

interface ILoginRequest {
    login: string;
    password: string;
}

interface ILoginResponse {
    token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

    private token: string;

    constructor(
        private httpService: HttpClient
    ) { }

    public login(user: IUser): Observable<void> {
        return this.httpService.post<ILoginResponse>(Endpoints.login, this.toServerUser(user))
            .pipe(
                map((result: ILoginResponse) => {
                    this.token = result.token;
                    localStorage.setItem('token', this.token);
                })
            )
    }

    public getHeaders(): HttpHeaders {
        return new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('token')}`);
    }

    private toServerUser(user: IUser): ILoginRequest {
        return {
            login: user.username,
            password: user.password
        }
    }

}
