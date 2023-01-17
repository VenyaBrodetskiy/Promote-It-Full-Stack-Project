import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class LoadingService {

    loading$ = new Subject<boolean>()

    public loadingOn() {
        this.loading$.next(true);
    }

    public loadingOff() {
        this.loading$.next(false);
    }
}
