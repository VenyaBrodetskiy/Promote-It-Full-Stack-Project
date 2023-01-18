import { Component } from '@angular/core';
import { ErrorService } from '../../services/error.service';

@Component({
    selector: 'bo-error',
    templateUrl: './error.component.html',
    styleUrls: ['./error.component.less']
})
export class ErrorComponent {
    constructor(
        public errorService: ErrorService
    ) { }

}
