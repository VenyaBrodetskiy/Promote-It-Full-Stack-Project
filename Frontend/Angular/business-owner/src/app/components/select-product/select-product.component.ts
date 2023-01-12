import { IProduct } from './../../models/product';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { v4 as uuidv4 } from "uuid";

@Component({
  selector: 'bo-select-product',
  templateUrl: './select-product.component.html',
  styleUrls: ['./select-product.component.less']
})
export class SelectProductComponent {

  @Input() options: IProduct[] | null;
  @Input() label: string;

  @Output() value: EventEmitter<number> = new EventEmitter<number>();

  public unique: string = uuidv4();

  public selectedOption: IProduct;

  public onChange(selectedOption: IProduct): void {
    this.value.emit(selectedOption.id);
  }

}
