import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ErrorService {
    public error$ = new Subject<string>()

    public handle(message: string) {
        this.error$.next(message)
    }

    public clear() {
        this.error$.next('')
    }

}
