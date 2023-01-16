import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class SuccessService {
    success$ = new Subject<string>()

    public handle(message: string) {
        this.success$.next(message)
        setTimeout(() => this.clear(), 2000);
    }

    public clear() {
        this.success$.next('')
    }

}
