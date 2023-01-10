import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ICampaign } from 'src/app/models/campaign';
import { CampaignService } from 'src/app/services/campaign.service';


@Component({
  selector: 'bo-campaign-page',
  templateUrl: './campaign-page.component.html',
  styleUrls: ['./campaign-page.component.less']
})
export class CampaignPageComponent implements OnInit {
  campaigns$: Observable<ICampaign[]>;

  constructor(private campaignService: CampaignService) {
  }

  ngOnInit(): void {
    this.campaigns$ = this.campaignService.getAll();
  }
}
