import { IProductForCampaignRow } from './../../models/table-line';
import { Component, Input, Output, SimpleChanges } from '@angular/core';
import { finalize, Observable } from 'rxjs';
import { ProductService } from 'src/app/services/product.service';
import { LoadingService } from 'src/app/services/loading.service';
import { EventEmitter } from '@angular/core';
import { NGXLogger } from 'ngx-logger';

@Component({
  selector: 'bo-products-of-campaign',
  templateUrl: './products-of-campaign.component.html',
  styleUrls: ['./products-of-campaign.component.less']
})
export class ProductsOfCampaignComponent {

    @Input() campaignId: number;
    @Input() userBalance: number;
    @Output() balanceChanged = new EventEmitter();
    public products: IProductForCampaignRow[];
    public rows$: Observable<IProductForCampaignRow[]>;
    public userBalanceForComponent: number;

    constructor(
        private productService: ProductService,
        private loadingService: LoadingService,
        private logger: NGXLogger
    ) { }

    public ngOnChanges(changes: SimpleChanges) {

        if (changes["campaignId"]) {
            let change = changes["campaignId"].currentValue;
            this.loadingService.loadingOn();
            this.rows$ = this.productService.getProductsForCampaign(change).pipe(
                finalize(() => {
                    this.loadingService.loadingOff();
                    this.logger.info(`Finished getting all products for campaignId: ${this.campaignId}`);
                })
            );
            this.logger.info(`campaignId was changed in product-of-campaign`, change);
        }

        if (changes["userBalance"]) {
            let change = changes["userBalance"].currentValue;
            this.userBalanceForComponent = change;
            this.logger.info(`userBalance was changed in product-of-campaign`, change);
        }
    }

    public checkBalance() {
        this.balanceChanged.emit();
        this.rows$ = this.productService.getProductsForCampaign(this.campaignId).pipe(
            finalize(() => {
                this.loadingService.loadingOff();
                this.logger.info(`Finished getting all products for campaignId: ${this.campaignId}`);
            })
        );
    }

}
