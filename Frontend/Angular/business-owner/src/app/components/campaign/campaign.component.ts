import { Component, Input } from '@angular/core'
import { ICampaign } from 'src/app/models/campaign'

@Component({
  selector: 'bo-campaign',
  templateUrl: './campaign.component.html'
})

export class CampaignComponent {
  @Input() campaign: ICampaign;

  donateproduct: boolean = false;
}
