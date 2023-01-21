import { Component } from '@angular/core';
import { finalize, Observable, Subject, takeUntil } from 'rxjs';
import { ICampaign } from 'src/app/models/campaign';
import { CampaignService } from 'src/app/services/campaign.service';
import { LoadingService } from 'src/app/services/loading.service';
import { NGXLogger } from 'ngx-logger';

@Component({
  selector: 'bo-campaign-np-page',
  templateUrl: './campaign-np-page.component.html',
  styleUrls: ['./campaign-np-page.component.less']
})
export class CampaignNpPageComponent {
    public campaigns$: Observable<ICampaign[]>;
    private unsubscribe$ = new Subject();

    constructor(
        private campaignService: CampaignService,
        private loadingService: LoadingService,
        private logger: NGXLogger
    ) { }

    public ngOnInit(): void {
        this.loadingService.loadingOn();
        this.campaigns$ = this.campaignService.getByNonProfitId().pipe(
            takeUntil(this.unsubscribe$),
            finalize(() => {
                this.loadingService.loadingOff();
                this.logger.info(`Finished getting all campaigns`);
            })
        );
    }

    public ngOnDestroy(): void {
        this.unsubscribe$.next(undefined);
        this.unsubscribe$.complete();
    }
}
