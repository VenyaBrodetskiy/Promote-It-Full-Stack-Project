import { IProductForCampaignRow } from './../../models/table-line';
import { AfterViewInit, Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { finalize, Observable } from 'rxjs';
import { ProductService } from 'src/app/services/product.service';
import { LoadingService } from 'src/app/services/loading.service';

@Component({
  selector: 'bo-products-of-campaign',
  templateUrl: './products-of-campaign.component.html',
  styleUrls: ['./products-of-campaign.component.less']
})
export class ProductsOfCampaignComponent {

    @Input() campaignId: number;
    products: IProductForCampaignRow[];
    rows$: Observable<IProductForCampaignRow[]>;

    constructor(
        private productService: ProductService,
        private loadingService: LoadingService,
    ) { }

    ngOnChanges() {
        if (this.campaignId) {
            this.loadingService.loadingOn();
            this.rows$ = this.productService.getProductsForCampaign(this.campaignId).pipe(
                finalize(() => this.loadingService.loadingOff())
            );
            console.log("onChanges", this.campaignId);
        }
    }

}
