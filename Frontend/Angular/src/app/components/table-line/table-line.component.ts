import { Component, Input } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { TransactionStates } from 'src/app/enums';
import { IProductForCampaignRow } from 'src/app/models/table-line';
import { ITransaction } from 'src/app/models/transaction';
import { ErrorService } from 'src/app/services/error.service';
import { TransactionService } from 'src/app/services/transaction.service';

@Component({
  selector: 'bo-table-line',
  templateUrl: './table-line.component.html',
  styleUrls: ['./table-line.component.less']
})
export class TableLineComponent {

    private unsubscribe$ = new Subject();
    TransactionStates = TransactionStates;
    @Input() row: IProductForCampaignRow;

    constructor(
        private transactionService: TransactionService,
        private errorService: ErrorService
    ) { }

    public onClick(StateId: TransactionStates): void {
        this.errorService.clear();
        let productId = this.row.productId;
        let campaignId = this.row.id;
        let body: ITransaction = {
            productId: productId,
            campaignId: campaignId,
            stateId: StateId
        }
        this.transactionService.create(body).pipe(
            takeUntil(this.unsubscribe$)
        )
            .subscribe({
                next: response => {
                    if (response.status === 200) {
                        console.log(response);
                    } else {
                        console.log("Error: ", response.status);
                    }
                },
                error: error => {
                    console.log("Error: ", error);
                }
            });
    }
}
