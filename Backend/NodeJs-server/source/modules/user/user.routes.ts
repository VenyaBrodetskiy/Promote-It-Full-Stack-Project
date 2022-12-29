import { RouteConfig } from '../../framework/routes.config';
import { Application } from "express"
import UserController from "./user.controller"
import AuthMiddleware from '../../core/middleware/auth.middleware';
import { UserType } from '../../common/enums';

export class UserRoutes extends RouteConfig {
    
    constructor(app: Application) {
        super(app, "UserRoutes", "api/user");
    }

    configureRoutes() {

        this.app.route(`/${this.baseUrl}`).post([
            UserController.addUser]);
        
        // this.app.route(`/${this.baseUrl}`).get([
        //     AuthMiddleware.verifyToken([UserType.businessOwner]), 
        //     UserController.getAll]);

        // this.app.route(`/${this.baseUrl}/:id`).put([
        //     AuthMiddleware.verifyToken([UserType.businessOwner]), 
        //     UserController.updateById]);

        // this.app.route(`/${this.baseUrl}/:id`).delete([
        //     AuthMiddleware.verifyToken([UserType.businessOwner]), 
        //     UserController.deleteById]);

        return this.app;
    }
}