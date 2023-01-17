import { Component } from '@angular/core';
import { defaultIfEmpty, finalize, map, Observable, Subject, tap } from 'rxjs';
import { IBalance } from 'src/app/models/balance';
import { ICampaign } from 'src/app/models/campaign';
import { BalanceService } from 'src/app/services/balance.service';
import { CampaignService } from 'src/app/services/campaign.service';
import { ErrorService } from 'src/app/services/error.service';
import { LoadingService } from 'src/app/services/loading.service';

@Component({
  selector: 'bo-balance-page',
  templateUrl: './balance-page.component.html',
  styleUrls: ['./balance-page.component.less']
})
export class BalancePageComponent {
    private unsubscribe$ = new Subject();
    public campaignId: number;
    public campaignOptions$: Observable<ICampaign[]>;
    public selectedCampaign: ICampaign;
    public campaignLabel: string = "Select a campaign";
    public zeroBalance: IBalance[] = [{
        id: 0,
        balance: 0
    }];
    public balance$: Observable<number>;
    //public balance: number;

    constructor(
        private campaignService: CampaignService,
        private errorService: ErrorService,
        private loadingService: LoadingService,
        private balanceService: BalanceService,
    ) { }

    public ngOnInit(): void {
        this.loadingService.loadingOn();
        this.campaignOptions$ = this.campaignService.getAll().pipe(
            finalize(() => this.loadingService.loadingOff())
        );
    }

    public ngOnDestroy(): void {
        this.unsubscribe$.next(undefined);
        this.unsubscribe$.complete();
    }

    public onCampaignSelected() {
        if (this.selectedCampaign) {
            this.campaignId = this.selectedCampaign.id;
            this.loadingService.loadingOn();
            this.balance$ = this.balanceService.getBalanceByCampaignId(this.campaignId).pipe(
                tap(balances => { if (!balances[0]) balances = this.zeroBalance }),
                map(balances => balances[0].balance),
                defaultIfEmpty(0),
                finalize(() => this.loadingService.loadingOff())
            );
            console.log(this.balance$);
        }
    }

    // ngOnChanges() {
    //     if (this.campaignId) {
    //         this.loadingService.loadingOn();
    //         this.balance$ = this.balanceService.getBalanceByCampaignId(this.campaignId).pipe(
    //             finalize(() => this.loadingService.loadingOff())
    //         );
    //         console.log("onChanges", this.campaignId);
    //     }
    // }

}
