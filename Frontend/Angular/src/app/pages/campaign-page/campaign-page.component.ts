import { Component, OnInit } from '@angular/core';
import { finalize, Observable } from 'rxjs';
import { ICampaign } from 'src/app/models/campaign';
import { CampaignService } from 'src/app/services/campaign.service';
import { ErrorService } from 'src/app/services/error.service';
import { LoadingService } from 'src/app/services/loading.service';


@Component({
    selector: 'bo-campaign-page',
    templateUrl: './campaign-page.component.html',
    styleUrls: ['./campaign-page.component.less']
})
export class CampaignPageComponent implements OnInit {
    campaigns$: Observable<ICampaign[]>;

    constructor(
        private campaignService: CampaignService,
        private errorService: ErrorService,
        private loadingService: LoadingService,
    ) {
    }

    public ngOnInit(): void {
        this.loadingService.loadingOn();
        this.campaigns$ = this.campaignService.getAll().pipe(
            finalize(() => this.loadingService.loadingOff())
        );
    }
}
