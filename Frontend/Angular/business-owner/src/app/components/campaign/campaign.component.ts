import { Component, Input } from '@angular/core'
import { States } from 'src/app/constants';
import { ICampaign } from 'src/app/models/campaign'
import { Router } from '@angular/router';

@Component({
  selector: 'bo-campaign',
  templateUrl: './campaign.component.html',
  styleUrls: ['./campaign.component.less']
})
export class CampaignComponent {

  constructor(private router: Router) { }

  @Input() campaign: ICampaign;

  States = States;

  onClick(campaignId: number) {
    this.router.navigate([States.donateToCampaign, { campaignId: campaignId }]);
  }
}
