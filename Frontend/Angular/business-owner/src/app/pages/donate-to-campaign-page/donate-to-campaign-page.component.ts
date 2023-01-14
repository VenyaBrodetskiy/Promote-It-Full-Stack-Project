import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { ICampaign } from 'src/app/models/campaign';
import { IDonation } from 'src/app/models/donation';
import { IProduct } from 'src/app/models/product';
import { CampaignService } from 'src/app/services/campaign.service';
import { DonationService } from 'src/app/services/donation.service';
import { ProductService } from 'src/app/services/product.service';
import { v4 as uuidv4 } from "uuid";

@Component({
  selector: 'bo-donate-to-campaign-page',
  templateUrl: './donate-to-campaign-page.component.html',
  styleUrls: ['./donate-to-campaign-page.component.less']
})
export class DonateToCampaignPageComponent {

    public campaignOptions$: Observable<ICampaign[]>;
    public selectedCampaign: ICampaign;
    public campaignLabel: string = "Select a campaign for donation";
    public productOptions$: Observable<IProduct[]>;
    public productLabel: string = "Select a product for donation";
    public selectedProduct: IProduct;
    public productQty: number;

    constructor(private route: ActivatedRoute,
        private campaignService: CampaignService,
        private productService: ProductService,
        private donationService: DonationService
    ) { }


    public ngOnInit(): void {

        this.campaignOptions$ = this.campaignService.getAll();
        this.productOptions$ = this.productService.getAll();
    }

    public onSubmit(selectedCampaign: ICampaign, selectedProduct: IProduct, productQty: number): void {
        let body: IDonation = {
            productId: selectedProduct.id,
            campaignId: selectedCampaign.id,
            productQty: productQty
        }
        this.donationService.create(body)
            .subscribe(
                response => {
                    console.log(response);
                }
            );
        console.log(body);
    }

}
