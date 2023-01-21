import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { NGXLogger } from 'ngx-logger';

@Injectable({
    providedIn: 'root'
})
export class LoadingService {

    public loading$ = new Subject<boolean>()

    constructor(
        private logger: NGXLogger
    ) { }

    public loadingOn() {
        this.logger.info(`Starting loading`);
        this.loading$.next(true);
    }

    public loadingOff() {
        this.logger.info(`Finishing loading`);
        this.loading$.next(false);
    }
}
