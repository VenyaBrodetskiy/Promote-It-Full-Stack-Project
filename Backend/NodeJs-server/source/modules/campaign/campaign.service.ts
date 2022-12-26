import { DateHelper } from "../../framework/date.helpers";
import { campaign, entityWithId, systemError } from "../../common/entities";
import { SqlHelper } from "../../core/helpers/sql.helper";
import { CampaignQueries } from "./campaign.queries";
import { Statuses } from "../../common/enums";

interface ICampaignService {
    addCampaign(campaign: campaign, userId: number): Promise<campaign>;
}


class CampaignService implements ICampaignService {

    constructor() {
    }

   
    public addCampaign(campaign: campaign, userId: number): Promise<campaign> {
        return new Promise<campaign>((resolve, reject) => {

            const createDate: string = DateHelper.dateToString(new Date());

            SqlHelper.createNew(
                CampaignQueries.AddCampaign, campaign,
                campaign.hashtag, campaign.landing_page, 
                createDate, createDate,
                userId, userId,
                Statuses.Active
            )
                .then((result: entityWithId) => {
                    resolve(result as campaign);
                })
                .catch((error: systemError) => reject(error));
        });
    }


}

export default new CampaignService();