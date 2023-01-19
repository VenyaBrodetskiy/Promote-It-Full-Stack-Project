import { Application } from "express"
import { UserType } from "../../common/enums";
import { RouteConfig } from '../../framework/routes.config';
import UserController from "./user.controller"
import AuthMiddleware from '../../core/middleware/auth.middleware';
export class UserRoutes extends RouteConfig {
    
    constructor(app: Application) {
        super(app, "UserRoutes", "api/user");
    }

    configureRoutes() {

        this.app.route(`/${this.baseUrl}/business-owner`).get([
            AuthMiddleware.verifyToken([UserType.system, UserType.prolobbyOwner]),
            UserController.getBusinessOwner]);

        this.app.route(`/${this.baseUrl}/social-activist`).get([
            AuthMiddleware.verifyToken([UserType.system, UserType.prolobbyOwner]),
            UserController.getSocialActivist]);

        this.app.route(`/${this.baseUrl}/nonprofit-organization`).get([
            AuthMiddleware.verifyToken([UserType.system, UserType.prolobbyOwner]),
            UserController.getNonProfitOrganization]);

        this.app.route(`/${this.baseUrl}/add-business-owner`).post([
            UserController.addBusinessOwner]);
        
        this.app.route(`/${this.baseUrl}/add-social-activist`).post([
            UserController.addSocialActivist]);
        
        this.app.route(`/${this.baseUrl}/add-nonprofit-organization`).post([
            UserController.addNonProfitOrganization]);

        return this.app;
    }
}