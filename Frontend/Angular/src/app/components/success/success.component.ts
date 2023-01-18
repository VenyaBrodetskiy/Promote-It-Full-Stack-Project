import { SuccessService } from './../../services/success.service';
import { Component } from '@angular/core';

@Component({
    selector: 'bo-success',
    templateUrl: './success.component.html',
    styleUrls: ['./success.component.less']
})
export class SuccessComponent {

    constructor(
        public successService: SuccessService
    ) { }
}
