import { Component } from '@angular/core';
import { finalize, Observable } from 'rxjs';
import { ICampaign } from 'src/app/models/campaign';
import { CampaignService } from 'src/app/services/campaign.service';
import { LoadingService } from 'src/app/services/loading.service';

@Component({
  selector: 'bo-campaign-np-page',
  templateUrl: './campaign-np-page.component.html',
  styleUrls: ['./campaign-np-page.component.less']
})
export class CampaignNpPageComponent {
    campaigns$: Observable<ICampaign[]>;

    constructor(
        private campaignService: CampaignService,
        private loadingService: LoadingService,
    ) {
    }

    ngOnInit(): void {
        this.loadingService.loadingOn();
        this.campaigns$ = this.campaignService.getByNonProfitId().pipe(
            finalize(() => this.loadingService.loadingOff())
        );
    }
}
