import { DateHelper } from "../../framework/date.helpers";
import { campaign, campaignsWitnProducts, entityWithId, systemError } from "../../common/entities";
import { SqlHelper } from "../../core/helpers/sql.helper";
import { CampaignQueries } from "./campaign.queries";
import { Statuses } from "../../common/enums";

interface ICampaignService {
    getAllCampaigns(): Promise<campaign[]>;
    getAllCampaignsWitnProducts(): Promise<campaignsWitnProducts[]>;
    addCampaign(campaign: campaign, userId: number): Promise<campaign>;
}


class CampaignService implements ICampaignService {

    constructor() {
    }

    public getAllCampaigns(): Promise<campaign[]> {
        return new Promise<campaign[]>((resolve, reject) => {
            const result: campaign[] = [];

            SqlHelper.executeQueryArrayResult<campaign>(CampaignQueries.GetAllCampaigns, Statuses.Active)
                .then((queryResult: campaign[]) => {
                    queryResult.forEach((campaign: campaign) => {
                        result.push(campaign)
                    });
                    resolve(result);
                })
                .catch((error: systemError) => reject(error));
        });
    }

    public getAllCampaignsWitnProducts(): Promise<campaignsWitnProducts[]> {
        return new Promise<campaignsWitnProducts[]>((resolve, reject) => {
            const result: campaignsWitnProducts[] = [];

            SqlHelper.executeQueryArrayResult<campaignsWitnProducts>(CampaignQueries.GetAllCampaignsWitnProducts, Statuses.Active)
                .then((queryResult: campaignsWitnProducts[]) => {
                    queryResult.forEach((campaign: campaignsWitnProducts) => {
                        result.push(campaign)
                    });
                    resolve(result);
                })
                .catch((error: systemError) => reject(error));
        });
    }
   
    public addCampaign(campaign: campaign, userId: number): Promise<campaign> {
        return new Promise<campaign>((resolve, reject) => {

            const createDate: string = DateHelper.dateToString(new Date());

            SqlHelper.createNew(
                CampaignQueries.AddCampaign, campaign,
                campaign.hashtag, campaign.landing_page, userId, 
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