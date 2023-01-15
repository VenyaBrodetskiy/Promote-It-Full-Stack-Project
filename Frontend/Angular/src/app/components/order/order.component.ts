import { Component, Input } from '@angular/core'
import { Subject, takeUntil } from 'rxjs';
import { States } from 'src/app/constants';
import { TransactionStates } from 'src/app/enums';
import { IChangeOrder, IOrder } from 'src/app/models/order';
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
        private orderService: OrderService
    ) { }

    public ngOnDestroy(): void {
        this.unsubscribe$.next(undefined);
        this.unsubscribe$.complete();
    }

    public onClick(orderId: number): void {
        this.isButtonDisabled = true;
        this.body = {
            id: orderId,
            stateId: TransactionStates.Shipped
        };
        this.orderService.changeState(this.body).pipe(
            takeUntil(this.unsubscribe$)
        )
            .subscribe(
                response => {
                    if (response.status === 200) {
                        this.order = response.body!;
                        console.log(response);
                    } else {
                        console.log("Error: ", response.status);
                    }
                },
                error => {
                    console.log("Error: ", error);
                }
            );
        console.log(orderId);
    }
}
