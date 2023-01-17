import { LoadingService } from 'src/app/services/loading.service';
import { Component, Input } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { TransactionStates } from 'src/app/enums';
import { IProductForCampaignRow } from 'src/app/models/table-line';
import { ITransaction } from 'src/app/models/transaction';
import { ErrorService } from 'src/app/services/error.service';
import { ProductService } from 'src/app/services/product.service';
import { SuccessService } from 'src/app/services/success.service';
import { TransactionService } from 'src/app/services/transaction.service';

@Component({
  selector: 'bo-table-line',
  templateUrl: './table-line.component.html',
  styleUrls: ['./table-line.component.less']
})
export class TableLineComponent {

    private unsubscribe$ = new Subject();
    public isRowAvailable: boolean = true;
    public TransactionStates = TransactionStates;
    @Input() row: IProductForCampaignRow | undefined;

    constructor(
        private transactionService: TransactionService,
        private errorService: ErrorService,
        private successService: SuccessService,
        private productService: ProductService,
        private loadingService: LoadingService,
    ) { }

    public onClick(StateId: TransactionStates): void {
        this.loadingService.loadingOn();
        this.errorService.clear();
        let productId = this.row!.productId;
        let campaignId = this.row!.id;
        let body: ITransaction = {
            productId: productId,
            campaignId: campaignId,
            stateId: StateId
        }
        let message: string;
        if (StateId === TransactionStates.Ordered) {
            message = "You successfully ordered product";
        } else if (StateId === TransactionStates.Donated) {
            message = "You successfully donated product";
        }
        this.transactionService.create(body).pipe(
            takeUntil(this.unsubscribe$)
        )
            .subscribe({
                next: response => {
                    if (response.status === 200) {
                        this.successService.handle(message, 10000);
                        this.productService.getProductsForCampaign(campaignId).pipe(
                            takeUntil(this.unsubscribe$)
                        )
                            .subscribe({
                                next: response => {
                                    if (response.length && response.find(product => product.id === productId))
                                        this.row = response.find(product => product.id === productId);
                                    else
                                        this.isRowAvailable = false;
                                }
                            });
                        console.log(response);
                    } else {
                        console.log("Error: ", response.status, response.body);
                    }
                    this.loadingService.loadingOff();
                },
                error: error => {
                    console.log("Error: ", error);
                    this.loadingService.loadingOff();
                }

            });

    }
}
