import { nonProfitOrganization, productsForCampaign } from './../../common/entities';
import { Statuses } from "../../common/enums";
import { campaign, campaignWitnProducts, entityWithId, systemError } from "../../common/entities";
import { CampaignQueries } from "./campaign.queries";
import { DateHelper } from "../../framework/date.helpers";
import { SqlHelper } from "../../core/helpers/sql.helper";

interface localCampaign extends entityWithId {
    hashtag: string;
    landing_page: string;
    non_profit_organization_name: string;
}

export interface localCampaignWitnProducts extends localCampaign {
    product_title: string;
    business_owner_name: string;
    product_qty: number;
}

interface localProductsForCampaign extends entityWithId {
    product_id: number;
    product_title: string;
    product_price: string;
    product_qty: string;
    campany_name: string;
}

interface ICampaignService {
    getAllCampaigns(): Promise<campaign[]>;
    getAllCampaignsWithProducts(): Promise<campaignWitnProducts[]>;
    getCampaignsByNonProfitId(nonProfitOrganizationId: number): Promise<campaign[]>
    addCampaign(campaign: campaign, userId: number): Promise<campaign>;
    getAllProductsForCampaign(campaignId: number): Promise<productsForCampaign[]>;
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
                        result.push(this.parseLocalCampaignWitnProducts(campaign))
                    });
                    resolve(result);
                })
                .catch((error: systemError) => reject(error));
        });
    }

    public getAllProductsForCampaign(campaignId: number): Promise<productsForCampaign[]> {
        return new Promise<productsForCampaign[]>((resolve, reject) => {
            const result: productsForCampaign[] = [];

            SqlHelper.executeQueryArrayResult<localProductsForCampaign>(CampaignQueries.GetAllProductsForCampaign, campaignId, Statuses.Active)
                .then((queryResult: localProductsForCampaign[]) => {
                    queryResult.forEach((obj: localProductsForCampaign) => {
                        result.push(this.parseLocalProductsForCampaign(obj))
                    });
                    resolve(result);
                })
                .catch((error: systemError) => reject(error));
        });
    }

    public getCampaignsByNonProfitId(nonProfitOrganizationId: number): Promise<campaign[]> {
        return new Promise<campaign[]>((resolve, reject) => {
            const result: campaign[] = [];

            SqlHelper.executeQueryArrayResult<localCampaign>(CampaignQueries.getCampaignsByNonProfitId, nonProfitOrganizationId, Statuses.Active)
                .then((queryResult: localCampaign[]) => {
                    queryResult.forEach((campaign: localCampaign) => {
                        result.push(this.parseLocalCampaign(campaign))
                    });
                    resolve(result);
                })
                .catch((error: systemError) => reject(error));
        });
    }
   
    public addCampaign(campaign: campaign, createUserId: number): Promise<campaign> {
        return new Promise<campaign>((resolve, reject) => {

            const createDate: string = DateHelper.dateToString(new Date());

            SqlHelper.createNew(
                CampaignQueries.AddCampaign, campaign,
                campaign.hashtag, campaign.landingPage, createUserId, 
                createDate, createDate,
                createUserId, createUserId,
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

    private parseLocalProductsForCampaign(result: localProductsForCampaign): productsForCampaign {
        return {
            id: result.id,
            productId: result.product_id,
            productTitle: result.product_title,
            productPrice: result.product_price,
            productQty: result.product_qty,
            companyName: result.campany_name
        }
    }

    private parseLocalCampaignWitnProducts(campaign: localCampaignWitnProducts): campaignWitnProducts {
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