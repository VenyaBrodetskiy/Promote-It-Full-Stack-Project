import { Application } from "express";
import { RouteConfig } from "../../framework/routes.config";
import { UserType } from "../../common/enums";
import AuthMiddleware from '../../core/middleware/auth.middleware';
import CampaignController from "./campaign.controller"

export class CampaignRoutes extends RouteConfig {

    constructor(app: Application) {
        super(app, "CampaignRoutes", "api/campaign");
    }

    configureRoutes() {

        this.app.route(`/${this.baseUrl}`).get([
            AuthMiddleware.verifyToken([UserType.system, UserType.businessOwner]),
            CampaignController.getAllCampaigns]);
        
        this.app.route(`/${this.baseUrl}/get-all/:id`).get([
            AuthMiddleware.verifyToken([UserType.system, UserType.socialActivist]),
            CampaignController.getAllProductsForCampaign]);

        this.app.route(`/${this.baseUrl}/product`).get([
            AuthMiddleware.verifyToken([UserType.system, UserType.prolobbyOwner, UserType.socialActivist]),
            CampaignController.getAllCampaignsWitnProducts]);

        this.app.route(`/${this.baseUrl}/non-profit`).get([
            AuthMiddleware.verifyToken([UserType.system, UserType.nonProfitOrganization]),
            CampaignController.getCampaignsByNonProfitId]);

        this.app.route(`/${this.baseUrl}`).post([
            AuthMiddleware.verifyToken([UserType.system, UserType.nonProfitOrganization]),
            CampaignController.addCampaign]);

        return this.app;
    }
}