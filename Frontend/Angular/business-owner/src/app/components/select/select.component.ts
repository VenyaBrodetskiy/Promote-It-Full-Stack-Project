import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ICampaign } from 'src/app/models/campaign';
import { v4 as uuidv4 } from "uuid";

@Component({
  selector: 'bo-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.less']
})
export class SelectComponent {


  @Input() options: ICampaign[] | null;
  @Input() label: string;

  @Output() value: EventEmitter<number> = new EventEmitter<number>();

  public unique: string = uuidv4();

  public selectedOption: ICampaign;

  public onChange(selectedOption: ICampaign): void {
    this.value.emit(selectedOption.id);
  }

}
