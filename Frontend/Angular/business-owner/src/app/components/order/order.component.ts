import { Component, Input } from '@angular/core'
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

    constructor(
        private orderService: OrderService
    ) { }

    @Input() orderNumber: string;
    @Input() order: IOrder;

    States = States;
    isButtonDisabled = false;
    body: IChangeOrder;

    public onClick(orderId: number): void {

        this.isButtonDisabled = true;
        this.body = {
            id: orderId,
            stateId: TransactionStates.Shipped
        };
        this.orderService.changeState(this.body)
            .subscribe(
                response => {
                    this.order = response;
                    console.log(response);
                }
            );
        console.log(orderId);
    }
}
