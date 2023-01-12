import { Component, Input } from '@angular/core'
import { States } from 'src/app/constants';
import { IOrder } from 'src/app/models/order';
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

    //TODO: UserId is hardcoded
  public onClick(orderId: number): void {

    this.isButtonDisabled = true;
    this.orderService.changeState(orderId)
      .subscribe(
        response => {
          this.order = response;
          console.log(response);
        }
      );
    console.log(orderId);
  }
}
