import { Component, Input } from '@angular/core'
import { Subject, takeUntil } from 'rxjs';
import { States } from 'src/app/constants';
import { TransactionStates } from 'src/app/enums';
import { IChangeOrder, IOrder } from 'src/app/models/order';
import { LoadingService } from 'src/app/services/loading.service';
import { OrderService } from 'src/app/services/order.service';
import { NGXLogger } from 'ngx-logger';


@Component({
    selector: 'bo-order',
    templateUrl: './order.component.html',
    styleUrls: ['./order.component.less']
})
export class OrderComponent {

    private unsubscribe$ = new Subject();
    @Input() orderNumber: string;
    @Input() order: IOrder;

    States = States;
    public isButtonDisabled = false;
    public body: IChangeOrder;

    constructor(
        private orderService: OrderService,
        private loadingService: LoadingService,
        private logger: NGXLogger
    ) { }

    public onClick(orderId: number): void {
        this.loadingService.loadingOn();
        this.isButtonDisabled = true;
        this.body = {
            id: orderId,
            stateId: TransactionStates.Shipped
        };
        this.orderService.changeState(this.body).pipe(
            takeUntil(this.unsubscribe$)
        )
            .subscribe({
                next: response => {
                    if (response.status === 200) {
                        this.order = response.body!;
                        this.logger.info(`Changes state of orderId: ${orderId}`, response.body);
                    } else {
                        this.logger.error(`Did not change state of orderId: ${orderId}`, response.status, response.body, response);
                        this.isButtonDisabled = false;
                    }
                    this.loadingService.loadingOff();
                },
                error: error => {
                    this.logger.error(`Did not change state of orderId: ${orderId}, ${error.message}`, error);
                    this.isButtonDisabled = false;
                    this.loadingService.loadingOff();
                }
            });

    }

    public ngOnDestroy(): void {
        this.unsubscribe$.next(undefined);
        this.unsubscribe$.complete();
    }
}
