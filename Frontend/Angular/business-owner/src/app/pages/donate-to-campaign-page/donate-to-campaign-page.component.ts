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

    constructor(private route: ActivatedRoute,
        private campaignService: CampaignService,
        private productService: ProductService,
        private donationService: DonationService
    ) { }

    @Input() campaignOptions: ICampaign[] | null;
    @Input() selectedCampaignId: number;
    @Input() compareWith: (c1: any, c2: any) => boolean;

    @Input() productOptions: IProduct[] | null;


    public unique: string = uuidv4();

    public selectedCampaign: ICampaign;

    public selectedProduct: IProduct;

    public campaignOptions$: Observable<ICampaign[]>;
    public campaignLabel: string = "Select a campaign for donation";
    //public selectedCampaignId: number;

    public productOptions$: Observable<IProduct[]>;
    public productLabel: string = "Select a product for donation";
    public selectedProductId: number;

    public productQty: number;

    ngOnInit(): void {

        this.campaignOptions$ = this.campaignService.getAll();
        this.productOptions$ = this.productService.getAll();
        this.route.paramMap.subscribe(params => {
            let campaignId = +this.route.snapshot.paramMap.get('campaignId')!;
            this.campaignOptions$.subscribe(options => {
                this.selectedCampaignId = options.find(c => c.id === campaignId)!.id;
            });
        });
    }

    //TODO: UserId is hardcoded
    public onSubmit(selectedCampaignId: number, selectedProductId: number, productQtyChange: number): void {
        let body: IDonation = {
            userId: 4,
            productId: selectedProductId,
            campaignId: selectedCampaignId,
            productQty: productQtyChange
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
