import { Component, OnInit } from '@angular/core';
import { finalize, Observable, Subject, takeUntil } from 'rxjs';
import { ICampaign } from 'src/app/models/campaign';
import { CampaignService } from 'src/app/services/campaign.service';
import { LoadingService } from 'src/app/services/loading.service';
import { NGXLogger } from 'ngx-logger';

@Component({
  selector: 'bo-campaign-sa-page',
  templateUrl: './campaign-sa-page.component.html',
  styleUrls: ['./campaign-sa-page.component.less']
})
export class CampaignSaPageComponent {
    public campaigns$: Observable<ICampaign[]>;
    private unsubscribe$ = new Subject();

    constructor(
        private campaignService: CampaignService,
        private loadingService: LoadingService,
        private logger: NGXLogger
    ) { }

    public ngOnInit(): void {
        this.loadingService.loadingOn();
        this.campaigns$ = this.campaignService.getAll().pipe(
            takeUntil(this.unsubscribe$),
            finalize(() => {
                this.loadingService.loadingOff()
                this.logger.info(`Finished getting all campaigns`);
            })
        );
    }

    public ngOnDestroy(): void {
        this.unsubscribe$.next(undefined);
        this.unsubscribe$.complete();
    }
}
