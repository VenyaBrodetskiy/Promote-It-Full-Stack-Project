import { Request, Response, NextFunction } from "express";
import { RequestHelper } from "../../core/helpers/request.helpers";
import { ResponseHelper } from "../../core/helpers/response.helper";
import { AuthenticatedRequest, systemError, user, userAnyType, userInfo } from "../../common/entities";
import { UserType, AppError } from "../../common/enums";
import UserService from "./user.service";
import ErrorService from "../../core/error.service";
import bcrypt from 'bcryptjs';
import { NON_EXISTING_ID, SYSTEM_ID } from "../../common/constants";
import loggerService from "../../core/logger.service";

class UserController {

    constructor() { }

    public addBusinessOwner(req: Request, res: Response, next: NextFunction) {

        loggerService.info(`${req.method} ${req.originalUrl}`);

        const user: user = UserController.parseBodyToUser(req);
        const userInfo: userInfo = UserController.parseBodyToUserInfo(req);
    
        UserService.addUser(user, SYSTEM_ID) 
            .then((id: number) => {
                userInfo.user_id = id;

                return UserService.addBusinessOwner(userInfo);
            })
            .then((userInfo: userInfo) => {
                return res.status(200).json(userInfo.user_id);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            }) 
    }

    public addSocialActivist(req: Request, res: Response, next: NextFunction) {

        loggerService.info(`${req.method} ${req.originalUrl}`);

        const user: user = UserController.parseBodyToUser(req);
        const userInfo: userInfo = UserController.parseBodyToUserInfo(req);

        UserService.addUser(user, SYSTEM_ID)
            .then((id: number) => {
                userInfo.user_id = id;

                return UserService.addSocialActivist(userInfo);
            })
            .then((userInfo: userInfo) => {
                return res.status(200).json(userInfo.user_id);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            })
    }
    
    private static parseBodyToUser(req: Request) {

        const body: userAnyType = req.body;
        const hashedPasword: string = bcrypt.hashSync(body.password as string);
        const user: user = {
            id: NON_EXISTING_ID,
            userTypeId: body.userTypeId,
            login: body.login,
            password: hashedPasword,
        };
        return user;
    }

    private static parseBodyToUserInfo(req: Request) {

        const body: userAnyType = req.body;
        const userInfo: userInfo = {
            user_id: NON_EXISTING_ID,
            twitter_handle: body.twitter_handle,
            name: body.name,
            email: body.email,
            address: body.address,
            phone_number: body.phone_number,
            website: body.website,
        };
        return userInfo;
    }
    
}

export default new UserController();