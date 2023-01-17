import { Request, Response, NextFunction } from "express";
import { NON_EXISTING_ID } from "../../common/constants";
import { campaign, AuthenticatedRequest, systemError, campaignWitnProducts, productsForCampaign } from "../../common/entities";
import CampaignService from "./campaign.service";
import LoggerService from "../../core/logger.service";
import { ResponseHelper } from "../../core/helpers/response.helper";
import { RequestHelper } from "../../core/helpers/request.helpers";

class CampaignController {

    constructor() { }

    public getAllCampaigns(req: Request, res: Response, next: NextFunction) {

        LoggerService.info(`${req.method} ${req.originalUrl}`);

        CampaignService.getAllCampaigns()
            .then((result: campaign[]) => {
                return res.status(200).json(result);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            })
    }

    public getAllCampaignsWitnProducts(req: Request, res: Response, next: NextFunction) {

        LoggerService.info(`${req.method} ${req.originalUrl}`);

        CampaignService.getAllCampaignsWithProducts()
            .then((result: campaignWitnProducts[]) => {
                return res.status(200).json(result);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            })
    }

    public getCampaignsByNonProfitId(req: Request, res: Response, next: NextFunction) {

        LoggerService.info(`${req.method} ${req.originalUrl}`);

        CampaignService.getCampaignsByNonProfitId((req as AuthenticatedRequest).userId)
            .then((result: campaign[]) => {
                return res.status(200).json(result);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            })
    }

    public getAllProductsForCampaign(req: Request, res: Response, next: NextFunction) {

        LoggerService.info(`${req.method} ${req.originalUrl}`);

        const numericParamOrError: number | systemError = RequestHelper.parseNumericInput(req.params.id);
        if (typeof numericParamOrError === "number") {

            if (numericParamOrError > 0) {
                CampaignService.GetAllProductsForCampaign(numericParamOrError)
                    .then((result: productsForCampaign[]) => {
                        return res.status(200).json(result);
                    })
                    .catch((error: systemError) => {
                        return ResponseHelper.handleError(res, error);
                    })
            }
            else {
                //TODO: 
            }
        }
        else {
            return ResponseHelper.handleError(res, numericParamOrError);
        }
    }

    public addCampaign(req: Request, res: Response, next: NextFunction) {

        // TODO: (very low priority) add validation, that landing page is working one 
        LoggerService.info(`${req.method} ${req.originalUrl}`);

        const body: campaign = req.body;
        const inputCampaign: campaign = {
            id: NON_EXISTING_ID,
            hashtag: body.hashtag,
            landingPage: body.landingPage
        };

        CampaignService.addCampaign(inputCampaign, (req as AuthenticatedRequest).userId)
            .then((result: campaign) => {
                return res.status(200).json(result);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            })
    }

}


export default new CampaignController();