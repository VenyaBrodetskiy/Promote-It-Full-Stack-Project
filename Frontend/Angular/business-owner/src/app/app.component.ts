import { Observable } from 'rxjs';
import { ICampaign } from './models/campaign';
import { Component, OnInit } from '@angular/core';
import { CampaignService } from './services/campaign.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {

  title = 'business-owner';

}
