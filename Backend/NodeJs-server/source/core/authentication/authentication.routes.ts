import { RouteConfig } from '../../framework/routes.config';
import { Application } from "express"
import AuthenticationController from "./authentication.controller"
import AuthMiddleware from '../../core/middleware/auth.middleware';
import { UserType } from '../../common/enums';

export class AuthenticationRoutes extends RouteConfig {
    
    constructor(app: Application) {
        super(app, "AuthenticationRoutes", "api/auth");
    }

    public configureRoutes() {
        this.app.route(`/${this.baseUrl}/login`).post([AuthenticationController.login]);

        this.app.route(`/${this.baseUrl}/user-type`).post([
            AuthMiddleware.verifyToken([UserType.businessOwner, UserType.socialActivist, UserType.nonProfitOrganization, UserType.prolobbyOwner, UserType.system]),
            AuthenticationController.getUserTypeId]);
        return this.app;
    }
}