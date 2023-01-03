import { Environment } from "./core/helpers/env.helper";
import * as http from "http";
import cors from "cors";
import morgan from "morgan";
import express, { Express } from "express";
import { RouteConfig } from "./framework/routes.config";
import { StoreRoutes } from "./modules/store/store.routes";
import { AuthenticationRoutes } from "./core/authentication/authentication.routes";
import { UserRoutes } from "./modules/user/user.routes";
import { RoleRoutes } from "./modules/role/role.routes";
import { EmployeeRoutes } from "./modules/employee/employee.routes";
import { CampaignRoutes } from "./modules/campaign/campaign.routes";
import LoggerService from "./core/logger.service";

LoggerService.init();

const routes: Array<RouteConfig> = [];
const app: Express = express();

/** Logging */
app.use(morgan('dev'));
/** Takes care of JSON data */
app.use(express.json());

app.use(cors());

routes.push(new AuthenticationRoutes(app));
routes.push(new UserRoutes(app));
routes.push(new RoleRoutes(app));
routes.push(new StoreRoutes(app));
routes.push(new EmployeeRoutes(app));
routes.push(new CampaignRoutes(app));

const server: http.Server = http.createServer(app);

server.listen(Environment.SERVER_PORT, () => {
    routes.forEach((route: RouteConfig) => {
        LoggerService.info(`Routes configured for ${route.getName()}`)
    });
    LoggerService.info(`Server is running on ${Environment.SERVER_PORT}`);
})