import { IProductForCampaignRow } from './../../models/table-line';
import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'bo-products-of-campaign',
  templateUrl: './products-of-campaign.component.html',
  styleUrls: ['./products-of-campaign.component.less']
})
export class ProductsOfCampaignComponent implements OnInit, AfterViewInit{

    @Input() campaignId: number;
    rows$: Observable<IProductForCampaignRow[]>;

    constructor(private productService: ProductService) {
        console.log("const", this.campaignId);

    }

    ngOnInit(): void {
        console.log("oninit",this.campaignId);
        this.rows$ = this.productService.getProductsForCampaign(this.campaignId);
    }

    ngAfterViewInit() {
        console.log("afterview", this.campaignId);

    }

}
