import { Component } from '@angular/core';
import { States } from "../../constants";

@Component({
  selector: 'bo-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.less']
})



export class MenuComponent {
  public States = States;
}
