// now is not used. created for checking if the problem is with addProduct in frontend or in backend, because same method, but different back
// addProduct - C#, addCampaign - nodeJS
// kept in case this front will be expanded to social activist. if not - need to remove


import { INewCampaign } from 'src/app/models/campaign';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { CampaignService } from 'src/app/services/campaign.service';

@Component({
  selector: 'bo-create-campaign',
  templateUrl: './create-campaign.component.html',
  styleUrls: ['./create-campaign.component.less']
})
export class CreateCampaignComponent {

  constructor(private campaignService: CampaignService) { }

  @Input() hashtag: string;
  @Input() landingPage: string;

  //TODO: UserId is hardcoded
  public onSubmit(hashtag: string, landingPage: string): void {
    let body: INewCampaign = {
      hashtag: hashtag,
      landingPage: landingPage
    }
    this.campaignService.create(body)
      .subscribe(
        response => {
          console.log(response);
        }
      );
    console.log(body);
  }

}
