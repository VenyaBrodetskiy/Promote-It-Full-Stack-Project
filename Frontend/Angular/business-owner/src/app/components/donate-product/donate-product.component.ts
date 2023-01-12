import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { ICampaign } from 'src/app/models/campaign';
import { IDonation } from 'src/app/models/donation';
import { IProduct } from 'src/app/models/product';
import { ActivatedRoute } from '@angular/router';

import { CampaignService } from 'src/app/services/campaign.service';
import { DonationService } from 'src/app/services/donation.service';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'bo-donate-product',
  templateUrl: './donate-product.component.html',
  styleUrls: ['./donate-product.component.less']
})
export class DonateProductComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private campaignService: CampaignService,
    private productService: ProductService,
    private donationService: DonationService) {
  }

  @Input() productQty: number;

  @Output() selectedCampaignIdChange: EventEmitter<number> = new EventEmitter<number>();
  @Output() selectedProductIdChange: EventEmitter<number> = new EventEmitter<number>();
  @Output() productQtyChange: EventEmitter<number> = new EventEmitter<number>();

  public campaignOptions$: Observable<ICampaign[]>;
  public campaignLabel: string = "Select a campaign for donation";
  public selectedCampaignId: number;

  public productOptions$: Observable<IProduct[]>;
  public productLabel: string = "Select a product for donation";
  public selectedProductId: number;


  public onSelectedCampaignIdChange($event: number): void {
    this.selectedCampaignId = $event;
    this.selectedCampaignIdChange.emit(this.selectedCampaignId);
  }

  public onSelectedProductIdChange(): void {
    this.selectedProductIdChange.emit(this.selectedProductId);
  }

  public onProductQtyChange(): void {
    this.productQtyChange.emit(this.productQty);
  }

  compareFn(c1: ICampaign, c2: ICampaign): boolean {
    return c1 && c2 ? c1.id === c2.id : c1 === c2;
  }

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
