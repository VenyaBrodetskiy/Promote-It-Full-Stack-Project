import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { INewProduct } from 'src/app/models/product';
import { ProductService } from 'src/app/services/product.service';
import { ErrorService } from 'src/app/services/error.service';

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
        private errorService: ErrorService
    ) {
        this.productTitleControl = new FormControl('', [Validators.required]);
        this.productPriceControl = new FormControl('', [Validators.required, this.isNaturalNumber]);
    }

    public ngOnDestroy(): void  {
        this.unsubscribe$.next(undefined);
        this.unsubscribe$.complete();
    }

    public onSubmit(): void {
        if (this.productTitleControl.valid && this.productPriceControl.valid) {
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
                .subscribe(
                    response => {
                        if (response.status === 200) {
                            this.toggle = !this.toggle;
                            this.productTitleControl.reset();
                            this.productPriceControl.reset();
                            console.log(response);
                        } else {
                            console.log("Error: ", response.status);
                        }
                    },
                    error => {
                        console.log("Error: ", error);
                    }
                );
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

}
