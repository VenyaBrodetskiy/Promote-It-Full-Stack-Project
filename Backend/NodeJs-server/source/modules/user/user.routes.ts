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

        this.app.route(`/${this.baseUrl}/add-business-owner`).post([
            UserController.addBusinessOwner]);
        
        this.app.route(`/${this.baseUrl}/add-social-activist`).post([
            UserController.addSocialActivist]);
        
        // сделать 3 эндпоинта (на каждый тип юзера)
        // на каждый эндопоинт свой контроллер, который делает 2 вещи: руководит записью в таблицу юзер, делает запись в "свою" таблицу
        // в сервисе 4 функции - по 1 на каждую таблицу

        return this.app;
    }
}