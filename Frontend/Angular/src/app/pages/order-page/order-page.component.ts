import { Component, OnInit } from '@angular/core';
import { finalize, Observable } from 'rxjs';
import { IOrder } from 'src/app/models/order';
import { LoadingService } from 'src/app/services/loading.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
    selector: 'bo-order-page',
    templateUrl: './order-page.component.html',
    styleUrls: ['./order-page.component.less']
})
export class OrderPageComponent implements OnInit {
    public orders$: Observable<IOrder[]>;

    constructor(
        private orderService: OrderService,
        private loadingService: LoadingService,
    ) { }

    public ngOnInit(): void {
        this.loadingService.loadingOn();
        this.orders$ = this.orderService.getAll().pipe(
            finalize(() => this.loadingService.loadingOff())
        );
    }
}

