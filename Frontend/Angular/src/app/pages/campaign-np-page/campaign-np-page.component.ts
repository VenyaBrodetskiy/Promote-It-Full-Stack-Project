import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { ICampaign } from 'src/app/models/campaign';
import { CampaignService } from 'src/app/services/campaign.service';

@Component({
  selector: 'bo-campaign-np-page',
  templateUrl: './campaign-np-page.component.html',
  styleUrls: ['./campaign-np-page.component.less']
})
export class CampaignNpPageComponent {
    campaigns$: Observable<ICampaign[]>;

    constructor(private campaignService: CampaignService) {
    }

    ngOnInit(): void {
        this.campaigns$ = this.campaignService.getByNonProfitId();
    }
}
