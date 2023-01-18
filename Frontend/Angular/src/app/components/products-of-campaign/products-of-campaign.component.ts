import { IProductForCampaignRow } from './../../models/table-line';
import { Component, Input, Output, SimpleChanges } from '@angular/core';
import { finalize, Observable } from 'rxjs';
import { ProductService } from 'src/app/services/product.service';
import { LoadingService } from 'src/app/services/loading.service';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'bo-products-of-campaign',
  templateUrl: './products-of-campaign.component.html',
  styleUrls: ['./products-of-campaign.component.less']
})
export class ProductsOfCampaignComponent {

    @Input() campaignId: number;
    @Input() userBalance: number;
    @Output() balanceChanged = new EventEmitter();
    products: IProductForCampaignRow[];
    rows$: Observable<IProductForCampaignRow[]>;
    public userBalanceForComponent: number;

    constructor(
        private productService: ProductService,
        private loadingService: LoadingService,
    ) { }

    public ngOnChanges(changes: SimpleChanges) {

        if (changes["campaignId"]) {
            let change = changes["campaignId"].currentValue;
            this.loadingService.loadingOn();
            this.rows$ = this.productService.getProductsForCampaign(change).pipe(
                finalize(() => this.loadingService.loadingOff())
            );
            console.log("onChanges", change);
        }

        if (changes["userBalance"]) {
            let change = changes["userBalance"].currentValue;
            this.userBalanceForComponent = change;
        }

    }

    public checkBalance() {
        this.balanceChanged.emit();
        this.rows$ = this.productService.getProductsForCampaign(this.campaignId).pipe(
            finalize(() => this.loadingService.loadingOff())
        );
    }

}
