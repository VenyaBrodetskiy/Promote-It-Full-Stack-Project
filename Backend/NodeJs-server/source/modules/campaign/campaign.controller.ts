import { Request, Response, NextFunction } from "express";
import { NON_EXISTING_ID } from "../../common/constants";
import { campaign, AuthenticatedRequest, systemError, campaignWitnProducts } from "../../common/entities";
import { ResponseHelper } from "../../core/helpers/response.helper";
import CampaignService from "./campaign.service";

class CampaignController {


    constructor() { }

    public getAllCampaigns(req: Request, res: Response, next: NextFunction) {
        CampaignService.getAllCampaigns()
            .then((result: campaign[]) => {
                return res.status(200).json(result);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            })
    }

    public getAllCampaignsWitnProducts(req: Request, res: Response, next: NextFunction) {
        CampaignService.getAllCampaignsWithProducts()
            .then((result: campaignWitnProducts[]) => {
                return res.status(200).json(result);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            })
    }

    public addCampaign(req: Request, res: Response, next: NextFunction) {

        // TODO: (high priority) add validation, that userId is type of non_profit organization

        // TODO: (very low priority) add validation, that landing page is working one 

        const body: campaign = req.body;
        const inputCampaign = {
            id: NON_EXISTING_ID,
            hashtag: body.hashtag,
            landingPage: body.landingPage
        };

        // TODO: АХТУНГ АДЫНАДЫН!!!11111!!!
        CampaignService.addCampaign(inputCampaign, 1) //(req as AuthenticatedRequest).userData.userId
            .then((result: campaign) => {
                return res.status(200).json(result);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            })
    }

}


export default new CampaignController();