import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { IProduct } from 'src/app/models/product';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'bo-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.less']
})
export class CreateProductComponent {

  constructor(private productService: ProductService) { }

  @Input() productTitle: string;
  @Input() productPrice: number;

  @Output() productTitleChange: EventEmitter<string> = new EventEmitter<string>();
  @Output() productPriceChange: EventEmitter<number> = new EventEmitter<number>();

  product$: Observable<number>;

  public onProductTitleChange(): void {
    this.productTitleChange.emit(this.productTitle);
  }

  public onProductPriceChange(): void {
    this.productPriceChange.emit(this.productPrice);
  }

  //TODO: UserId is hardcoded
  public onSubmit(productTitle: string, productPrice: number): void {
    let body: IProduct = {
      title: productTitle,
      price: productPrice,
      userId: 4
    }
    this.productService.create(body)
      .subscribe(
        response => {
          console.log(response);
        }
      );
    console.log(body);
  }



}
