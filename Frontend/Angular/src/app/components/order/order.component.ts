import { Component, Input } from '@angular/core'
import { Subject, takeUntil } from 'rxjs';
import { States } from 'src/app/constants';
import { TransactionStates } from 'src/app/enums';
import { IChangeOrder, IOrder } from 'src/app/models/order';
import { LoadingService } from 'src/app/services/loading.service';
import { OrderService } from 'src/app/services/order.service';

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
    ) { }

    public ngOnDestroy(): void {
        this.unsubscribe$.next(undefined);
        this.unsubscribe$.complete();
    }

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
                        console.log(response);
                    } else {
                        console.log("Error: ", response.status, response.body);
                    }
                    this.loadingService.loadingOn();
                },
                error: error => {
                    console.log("Error: ", error);
                    this.loadingService.loadingOn();
                }
            });
        console.log(orderId);
    }
}
