import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { ICampaign } from 'src/app/models/campaign';

import { CampaignService } from 'src/app/services/campaign.service';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'bo-donate-product',
  templateUrl: './donate-product.component.html',
  styleUrls: ['./donate-product.component.less']
})
export class DonateProductComponent implements OnInit {

  constructor(private campaignService: CampaignService,
    private productService: ProductService) {
  }

  @Output() selectedCampaignIdChange: EventEmitter<number> = new EventEmitter<number>();


  public campaignOptions$: Observable<ICampaign[]>;
  public campaignLabel: string;
  public selectedCampaignId: number;


  public onSelectedCampaignIdChange(): void {
    this.selectedCampaignIdChange.emit(this.selectedCampaignId);
  }


  ngOnInit(): void {

    this.campaignOptions$ = this.campaignService.getAll();
  }

}
