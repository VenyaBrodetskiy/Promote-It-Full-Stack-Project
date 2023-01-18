import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class AuthStateService {
    private isLoggedIn = new BehaviorSubject<boolean>(false);

    public getIsLoggedIn() {
        return this.isLoggedIn.asObservable();
    }

    public setIsLoggedIn(value: boolean) {
        this.isLoggedIn.next(value);
    }
}
