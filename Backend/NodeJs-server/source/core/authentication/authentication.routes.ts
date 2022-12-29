import { RouteConfig } from '../../framework/routes.config';
import { Application } from "express"
import AuthenticationController from "./authentication.controller"

export class AuthenticationRoutes extends RouteConfig {
    
    constructor(app: Application) {
        // TODO: change baseUrl to "api/[controller]"

        super(app, "AuthenticationRoutes", "auth");
    }

    public configureRoutes() {
        this.app.route(`/${this.baseUrl}/login`).post([AuthenticationController.login]);
        return this.app;
    }
}