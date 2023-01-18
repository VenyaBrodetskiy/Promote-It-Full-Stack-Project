import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class SuccessService {
    public success$ = new Subject<string>()

    public handle(message: string, time: number) {
        this.success$.next(message)
        setTimeout(() => this.clear(), time);
    }

    public clear() {
        this.success$.next('')
    }

}
