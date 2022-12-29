import { DateHelper } from "../../framework/date.helpers";
import { campaign, campaignWitnProducts, entityWithId, systemError } from "../../common/entities";
import { SqlHelper } from "../../core/helpers/sql.helper";
import { CampaignQueries } from "./campaign.queries";
import { Statuses } from "../../common/enums";

interface localCampaign extends entityWithId {
    hashtag: string;
    landing_page: string;
    non_profit_organization_name?: string;
}

export interface localCampaignWitnProducts extends localCampaign {
    product_title: string;
    business_owner_name: string;
    product_qty: number;

}

interface ICampaignService {
    getAllCampaigns(): Promise<campaign[]>;
    getAllCampaignsWithProducts(): Promise<campaignWitnProducts[]>;
    addCampaign(campaign: campaign, userId: number): Promise<campaign>;
}


class CampaignService implements ICampaignService {

    constructor() {
    }

    public getAllCampaigns(): Promise<campaign[]> {
        return new Promise<campaign[]>((resolve, reject) => {
            const result: campaign[] = [];

            SqlHelper.executeQueryArrayResult<localCampaign>(CampaignQueries.GetAllCampaigns, Statuses.Active)
                .then((queryResult: localCampaign[]) => {
                    queryResult.forEach((campaign: localCampaign) => {
                        result.push(this.parseLocalCampaign(campaign))
                    });
                    resolve(result);
                })
                .catch((error: systemError) => reject(error));
        });
    }

    public getAllCampaignsWithProducts(): Promise<campaignWitnProducts[]> {
        return new Promise<campaignWitnProducts[]>((resolve, reject) => {
            const result: campaignWitnProducts[] = [];

            SqlHelper.executeQueryArrayResult<localCampaignWitnProducts>(CampaignQueries.GetAllCampaignsWitnProducts, Statuses.Active)
                .then((queryResult: localCampaignWitnProducts[]) => {
                    queryResult.forEach((campaign: localCampaignWitnProducts) => {
                        result.push(this.localCampaignWitnProducts(campaign))
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
                campaign.hashtag, campaign.landingPage, userId, 
                createDate, createDate,
                userId, userId,
                Statuses.Active)
                .then((result: entityWithId) => {
                    resolve(result as campaign);
                })
                .catch((error: systemError) => reject(error));
        });
    }

    
    private parseLocalCampaign(campaign: localCampaign): campaign {
        return {
            id: campaign.id,
            hashtag: campaign.hashtag,
            landingPage: campaign.landing_page,
            nonProfitOrganizationName: campaign.non_profit_organization_name,
        }
    }

    private localCampaignWitnProducts(campaign: localCampaignWitnProducts): campaignWitnProducts {
        return {
            id: campaign.id,
            hashtag: campaign.hashtag,
            landingPage: campaign.landing_page,
            nonProfitOrganizationName: campaign.non_profit_organization_name,
            productTitle: campaign.product_title,
            businessOwnerName: campaign.business_owner_name,
            productQty: campaign.product_qty
        }
    }


}

export default new CampaignService();