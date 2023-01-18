import { Component, EventEmitter, Output } from '@angular/core';
import { defaultIfEmpty, finalize, map, Observable, Subject } from 'rxjs';
import { IBalance } from 'src/app/models/balance';
import { ICampaign } from 'src/app/models/campaign';
import { BalanceService } from 'src/app/services/balance.service';
import { CampaignService } from 'src/app/services/campaign.service';
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


    constructor(
        private campaignService: CampaignService,
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
                defaultIfEmpty(this.zeroBalance),
                map(balances => {
                    if (balances.length > 0) {
                        return balances[0].balance
                    } else {
                        return this.zeroBalance[0].balance
                    }
                }),
                finalize(() => this.loadingService.loadingOff())
            );
            console.log(this.balance$);
        }
    }

    public changedBalance() {
        this.loadingService.loadingOn();
        this.balance$ = this.balanceService.getBalanceByCampaignId(this.campaignId).pipe(
            defaultIfEmpty(this.zeroBalance),
            map(balances => {
                if (balances.length > 0) {
                    return balances[0].balance
                } else {
                    return this.zeroBalance[0].balance
                }
            }),
            finalize(() => this.loadingService.loadingOff())
        );
        console.log(this.balance$);
    }

    // public checkBalance() {
    //     this.loadingService.loadingOn();
    //     this.balance$ = this.balanceService.getBalanceByCampaignId(this.campaignId).pipe(
    //         defaultIfEmpty(this.zeroBalance),
    //         map(balances => {
    //             if (balances.length > 0) {
    //                 return balances[0].balance
    //             } else {
    //                 return this.zeroBalance[0].balance
    //             }
    //         }),
    //         finalize(() => this.loadingService.loadingOff())
    //     );
    //     console.log(this.balance$);
    // }

}
