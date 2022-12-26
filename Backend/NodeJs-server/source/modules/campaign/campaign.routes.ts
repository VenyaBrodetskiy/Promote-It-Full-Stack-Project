import { Application } from "express";
import { RouteConfig } from "../../framework/routes.config";
import CampaignController from "./campaign.controller"

export class CampaignRoutes extends RouteConfig {

    constructor(app: Application) {
        super(app, "CampaignRoutes", "campaign");
    }

    configureRoutes() {

        this.app.route(`/${this.baseUrl}`).post(
            CampaignController.addCampaign);

        return this.app;
    }
}