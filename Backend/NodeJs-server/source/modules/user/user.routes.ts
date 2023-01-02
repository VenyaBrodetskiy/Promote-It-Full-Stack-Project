import { Application } from "express"
import { RouteConfig } from '../../framework/routes.config';
import UserController from "./user.controller"

export class UserRoutes extends RouteConfig {
    
    constructor(app: Application) {
        super(app, "UserRoutes", "api/user");
    }

    configureRoutes() {

        this.app.route(`/${this.baseUrl}/add-business-owner`).post([
            UserController.addBusinessOwner]);
        
        this.app.route(`/${this.baseUrl}/add-social-activist`).post([
            UserController.addSocialActivist]);
        
        this.app.route(`/${this.baseUrl}/add-nonprofit-organization`).post([
            UserController.addNonProfitOrganization]);

        return this.app;
    }
}