import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { INewProduct } from 'src/app/models/product';
import { ProductService } from 'src/app/services/product.service';
import { ErrorService } from 'src/app/services/error.service';
import { LoadingService } from 'src/app/services/loading.service';
import { NGXLogger } from 'ngx-logger';

@Component({
    selector: 'bo-create-product',
    templateUrl: './create-product.component.html',
    styleUrls: ['./create-product.component.less']
})
export class CreateProductComponent {

    private unsubscribe$ = new Subject();
    public toggle: boolean = true;
    public productTitleControl: FormControl;
    public productPriceControl: FormControl;
    public productTitle: string = '';
    public productPrice: number;

    constructor(
        private productService: ProductService,
        private errorService: ErrorService,
        private loadingService: LoadingService,
        private logger: NGXLogger
    ) {
        this.productTitleControl = new FormControl('', [Validators.required]);
        this.productPriceControl = new FormControl('', [Validators.required, this.isNaturalNumber]);
    }

    public onSubmit(): void {
        if (this.productTitleControl.valid && this.productPriceControl.valid) {
            this.loadingService.loadingOn();
            this.errorService.clear();
            let productTitle = this.productTitleControl.value;
            let productPrice = this.productPriceControl.value;
            let body: INewProduct = {
                title: productTitle,
                price: productPrice
            }
            this.productService.create(body).pipe(
                takeUntil(this.unsubscribe$)
            )
                .subscribe({
                    next: response => {
                        if (response.status === 200) {
                            this.logger.info(`Added product: `, response.body);
                            this.toggle = !this.toggle;
                            this.productTitleControl.reset();
                            this.productPriceControl.reset();
                        } else {
                            this.logger.error(`Did not add product: `, response.status, response.body, response);
                        }
                        this.loadingService.loadingOff();
                    },
                    error: error => {
                        this.logger.error(`Did not add product: ${error.message}`, error);
                        this.loadingService.loadingOff();
                    }
                });
        }
    }

    public onSubmitMore(): void {
        this.toggle = !this.toggle;
    }

    private isNaturalNumber(control: FormControl) {
        const value = control.value;
        if (!value || value < 0) {
            return { naturalNumber: true };
        }
        return null;
    }

    public ngOnDestroy(): void {
        this.unsubscribe$.next(undefined);
        this.unsubscribe$.complete();
    }
}
