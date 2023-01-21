import { INewCampaign } from 'src/app/models/campaign';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable, Subject, takeUntil } from 'rxjs';
import { CampaignService } from 'src/app/services/campaign.service';
import { ErrorService } from 'src/app/services/error.service';
import { FormControl, Validators } from '@angular/forms';
import { LoadingService } from 'src/app/services/loading.service';
import { NGXLogger } from 'ngx-logger';

@Component({
    selector: 'bo-create-campaign',
    templateUrl: './create-campaign.component.html',
    styleUrls: ['./create-campaign.component.less']
})
export class CreateCampaignComponent {

    private unsubscribe$ = new Subject();
    public toggle: boolean = true;
    public hashtagControl: FormControl;
    public landingPageControl: FormControl;
    public hashtag: string = '';
    public landingPage: string = '';

    constructor(
        private campaignService: CampaignService,
        private errorService: ErrorService,
        private loadingService: LoadingService,
        private logger: NGXLogger
    ) {
        this.hashtagControl = new FormControl('', [Validators.required, Validators.pattern(/^#[a-zA-Z0-9_]{1,280}$/)]);
        this.landingPageControl = new FormControl('', [Validators.required, Validators.pattern(/^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/)]);
    }

    public onSubmit(): void {
        this.loadingService.loadingOn();
        if (this.hashtagControl.valid && this.landingPageControl.valid) {
            this.errorService.clear();
            let hashtag = this.hashtagControl.value;
            let landingPage = this.landingPageControl.value;
            let body: INewCampaign = {
                hashtag: hashtag,
                landingPage: landingPage
            }
            this.campaignService.create(body).pipe(
                takeUntil(this.unsubscribe$)
            )
                .subscribe({
                    next: response => {
                        if (response.status === 200) {
                            this.logger.info(`Added campaign: ${body}`, response.body);
                            this.toggle = !this.toggle;
                            this.hashtagControl.reset();
                            this.landingPageControl.reset();
                        } else {
                            this.logger.error(`Did not add campaign: ${body}`, response.status, response.body, response);
                        }
                        this.loadingService.loadingOff();
                    },
                    error: error => {
                        this.logger.error(`Did not add campaign: ${body}, ${error.message}`, error);
                        this.loadingService.loadingOff();
                    }
                });
        }
    }

    public onSubmitMore(): void {
        this.toggle = !this.toggle;
    }

    public ngOnDestroy(): void {
        this.unsubscribe$.next(undefined);
        this.unsubscribe$.complete();
    }
}
