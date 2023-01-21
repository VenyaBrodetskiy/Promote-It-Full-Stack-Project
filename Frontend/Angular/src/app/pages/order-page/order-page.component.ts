import { Component, OnInit } from '@angular/core';
import { finalize, Observable, Subject, takeUntil } from 'rxjs';
import { IOrder } from 'src/app/models/order';
import { LoadingService } from 'src/app/services/loading.service';
import { OrderService } from 'src/app/services/order.service';
import { NGXLogger } from 'ngx-logger';

@Component({
    selector: 'bo-order-page',
    templateUrl: './order-page.component.html',
    styleUrls: ['./order-page.component.less']
})
export class OrderPageComponent implements OnInit {
    public orders$: Observable<IOrder[]>;
    private unsubscribe$ = new Subject();

    constructor(
        private orderService: OrderService,
        private loadingService: LoadingService,
        private logger: NGXLogger
    ) { }

    public ngOnInit(): void {
        this.loadingService.loadingOn();
        this.orders$ = this.orderService.getAll().pipe(
            takeUntil(this.unsubscribe$),
            finalize(() => {
                this.loadingService.loadingOff();
                this.logger.info(`Finished getting all orders`);
            })
        );
    }
}

