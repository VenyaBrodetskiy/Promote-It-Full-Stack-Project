import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { NGXLogger } from 'ngx-logger';

@Injectable({
    providedIn: 'root'
})
export class ErrorService {
    public error$ = new Subject<string>()

    constructor(
        private logger: NGXLogger
    ) { }

    public handle(message: string) {
        this.logger.error(`Handling error: ${message}`);
        this.error$.next(message)
    }

    public clear() {
        this.logger.info(`Clearing the error`);
        this.error$.next('')
    }

}
