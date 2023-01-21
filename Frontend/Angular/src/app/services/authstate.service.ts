import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { NGXLogger } from 'ngx-logger';

@Injectable({
    providedIn: 'root'
})
export class AuthStateService {
    private isLoggedIn = new BehaviorSubject<boolean>(false);

    constructor(
        private logger: NGXLogger
    ) { }

    public getIsLoggedIn() {
        this.logger.info(`Getting isLoggedIn`);
        return this.isLoggedIn.asObservable();
    }

    public setIsLoggedIn(value: boolean) {
        this.logger.info(`Setting isLoggedIn: `,value);
        this.isLoggedIn.next(value);
    }
}
