import { Observable } from 'rxjs';
import { ICampaign } from './models/campaign';
import { Component, OnInit } from '@angular/core';
import { CampaignService } from './services/campaign.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent implements OnInit {

  title = 'business-owner';
  campaigns$: Observable<ICampaign[]>;

  constructor(private campaignService: CampaignService) {
  }

  ngOnInit(): void {
    this.campaigns$ = this.campaignService.getAll();
  }
}
