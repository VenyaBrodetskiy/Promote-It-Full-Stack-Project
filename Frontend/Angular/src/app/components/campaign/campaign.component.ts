import { Component, Input } from '@angular/core'
import { ICampaign } from 'src/app/models/campaign'

@Component({
    selector: 'bo-campaign',
    templateUrl: './campaign.component.html',
    styleUrls: ['./campaign.component.less']
})
export class CampaignComponent {

    @Input() campaign: ICampaign;

    constructor() { }

}
