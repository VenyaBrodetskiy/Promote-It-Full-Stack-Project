import { Application } from "express";
import { RouteConfig } from "../../framework/routes.config";
import CampaignController from "./campaign.controller"
import AuthMiddleware from '../../core/middleware/auth.middleware';
import { UserType } from "../../common/enums";

export class CampaignRoutes extends RouteConfig {

    constructor(app: Application) {
        super(app, "CampaignRoutes", "api/campaign");
    }

    configureRoutes() {

        this.app.route(`/${this.baseUrl}`).get([
            AuthMiddleware.verifyToken(UserType.businessOwner),
            CampaignController.getAllCampaigns]);
        
        this.app.route(`/${this.baseUrl}/product`).get(
            CampaignController.getAllCampaignsWitnProducts);

        this.app.route(`/${this.baseUrl}`).post(
            CampaignController.addCampaign);
        

        return this.app;
    }
}