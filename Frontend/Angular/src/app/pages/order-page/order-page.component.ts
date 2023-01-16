import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IOrder } from 'src/app/models/order';
import { OrderService } from 'src/app/services/order.service';

@Component({
    selector: 'bo-order-page',
    templateUrl: './order-page.component.html',
    styleUrls: ['./order-page.component.less']
})
export class OrderPageComponent implements OnInit {
    orders$: Observable<IOrder[]>;

    constructor(private orderService: OrderService) {
    }

    ngOnInit(): void {
        this.orders$ = this.orderService.getAll();
    }
}

