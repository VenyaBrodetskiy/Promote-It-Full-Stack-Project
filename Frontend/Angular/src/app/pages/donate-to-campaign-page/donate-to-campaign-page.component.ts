import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { finalize, Observable, Subject, takeUntil } from 'rxjs';
import { ICampaign } from 'src/app/models/campaign';
import { IDonation } from 'src/app/models/donation';
import { IProduct } from 'src/app/models/product';
import { CampaignService } from 'src/app/services/campaign.service';
import { DonationService } from 'src/app/services/donation.service';
import { ErrorService } from 'src/app/services/error.service';
import { LoadingService } from 'src/app/services/loading.service';
import { ProductService } from 'src/app/services/product.service';
import { NGXLogger } from 'ngx-logger';

@Component({
    selector: 'bo-donate-to-campaign-page',
    templateUrl: './donate-to-campaign-page.component.html',
    styleUrls: ['./donate-to-campaign-page.component.less']
})
export class DonateToCampaignPageComponent {

    private unsubscribe$ = new Subject();
    public formValid = false;
    public toggle: boolean = true;
    public campaignOptions$: Observable<ICampaign[]>;
    public selectedCampaign: ICampaign;
    public campaignLabel: string = "Select a campaign for donation";
    public productOptions$: Observable<IProduct[]>;
    public productLabel: string = "Select a product for donation";
    public selectedProduct: IProduct;
    public productQty: number;
    public productQtyControl: FormControl;

    constructor(
        private campaignService: CampaignService,
        private productService: ProductService,
        private donationService: DonationService,
        private errorService: ErrorService,
        private loadingService: LoadingService,
        private logger: NGXLogger
    ) {
        this.productQtyControl = new FormControl('', [Validators.required, this.isNaturalNumber]);
    }


    public ngOnInit(): void {
        this.loadingService.loadingOn();
        this.campaignOptions$ = this.campaignService.getAll().pipe(
            takeUntil(this.unsubscribe$),
            finalize(() => {
                this.loadingService.loadingOff();
                this.logger.info(`Finished getting all campaigns`);
            })
        );
        this.productOptions$ = this.productService.getAll().pipe(
            takeUntil(this.unsubscribe$),
            finalize(() => {
                this.loadingService.loadingOff();
                this.logger.info(`Finished getting all products`);
            })
        );
    }

    public ngOnDestroy(): void {
        this.unsubscribe$.next(undefined);
        this.unsubscribe$.complete();
    }

    public onSubmit(selectedCampaign: ICampaign, selectedProduct: IProduct, productQty: number): void {
        if (this.productQtyControl.valid) {
            this.loadingService.loadingOn();
            this.errorService.clear();
            let body: IDonation = {
                productId: selectedProduct.id,
                campaignId: selectedCampaign.id,
                productQty: productQty
            }
            this.donationService.create(body).pipe(
                takeUntil(this.unsubscribe$)
            )
                .subscribe({
                    next: response => {
                        if (response.status === 200) {
                            this.toggle = !this.toggle;
                            this.productQtyControl.reset();
                            this.logger.info(`Added donation: `, response.body);
                        } else {
                            this.logger.error(`Did not add donation: `, response.status, response.body, response);
                        }
                        this.loadingService.loadingOff();
                    },
                    error: error => {
                        this.logger.error(`Did not add donation: `, error.message, error);
                        this.loadingService.loadingOff();
                    }
                });
        }
    }

    public onSubmitMore(): void {
        this.toggle = !this.toggle;
        this.errorService.clear();
    }

    public onCampaignSelected() {
        this.formValid = this.selectedCampaign ? (this.selectedProduct ? true : false) : false;
    }

    public onProductSelected() {
        this.formValid = this.selectedCampaign ? (this.selectedProduct ? true : false) : false;
    }

    private isNaturalNumber(control: FormControl) {
        const value = control.value;
        if (!value || value < 0) {
            return { naturalNumber: true };
        }
        return null;
    }
}
