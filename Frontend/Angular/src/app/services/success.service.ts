import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { NGXLogger } from 'ngx-logger';


@Injectable({
    providedIn: 'root'
})
export class SuccessService {
    public success$ = new Subject<string>()

    constructor(
        private logger: NGXLogger
    ) { }

    public handle(message: string, time: number) {
        this.logger.info(`Handling success message: `, message);
        this.success$.next(message)
        setTimeout(() => this.clear(), time);
    }

    public clear() {
        this.logger.info(`Clearing success message`);
        this.success$.next('')
    }

}
