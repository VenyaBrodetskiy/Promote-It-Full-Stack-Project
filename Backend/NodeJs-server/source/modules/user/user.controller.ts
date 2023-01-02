import { Request, Response, NextFunction } from "express";
import bcrypt from 'bcryptjs';
import { NON_EXISTING_ID, SYSTEM_ID } from "../../common/constants";
import { UserType, AppError } from "../../common/enums";
import { systemError, businessOwner, socialActivist, nonProfitOrganization } from "../../common/entities";
import UserService from "./user.service";
import ErrorService from "../../core/error.service";
import LoggerService from "../../core/logger.service";
import { ResponseHelper } from "../../core/helpers/response.helper";

class UserController {

    constructor() { }

    public addBusinessOwner(req: Request, res: Response, next: NextFunction) {

        LoggerService.info(`${req.method} ${req.originalUrl}`);

        const body: businessOwner = req.body;
        const hashedPasword: string = bcrypt.hashSync(body.password as string);
        const businessOwner: businessOwner = {
            id: NON_EXISTING_ID,
            userTypeId: body.userTypeId,
            login: body.login,
            password: hashedPasword,
            user_id: NON_EXISTING_ID,
            twitter_handle: body.twitter_handle,
            name: body.name,
            email: body.email,
        };

        if (businessOwner.userTypeId !== UserType.businessOwner) {
            return ResponseHelper.handleError(res, ErrorService.getError(AppError.InputParameterNotSupplied));
        }

        UserService.addUser(businessOwner, SYSTEM_ID) 
            .then((id: number) => {
                businessOwner.user_id = id;
                return UserService.addBusinessOwner(businessOwner);
            })
            .then(() => {
                return res.status(200).json(businessOwner.user_id);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            }) 
    }

    public addSocialActivist(req: Request, res: Response, next: NextFunction) {

        LoggerService.info(`${req.method} ${req.originalUrl}`);

        const body: socialActivist = req.body;
        const hashedPasword: string = bcrypt.hashSync(body.password as string);
        const socialActivist: socialActivist = {
            id: NON_EXISTING_ID,
            userTypeId: body.userTypeId,
            login: body.login,
            password: hashedPasword,
            user_id: NON_EXISTING_ID,
            twitter_handle: body.twitter_handle,
            email: body.email,
            address: body.address,
            phone_number: body.phone_number,
        };

        if (socialActivist.userTypeId !== UserType.socialActivist) {
            return ResponseHelper.handleError(res, ErrorService.getError(AppError.InputParameterNotSupplied));
        }

        UserService.addUser(socialActivist, SYSTEM_ID)
            .then((id: number) => {
                socialActivist.user_id = id;
                return UserService.addSocialActivist(socialActivist);
            })
            .then(() => {
                return res.status(200).json(socialActivist.user_id);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            })
    }

    public addNonProfitOrganization(req: Request, res: Response, next: NextFunction) {

        LoggerService.info(`${req.method} ${req.originalUrl}`);

        const body: nonProfitOrganization = req.body;
        const hashedPasword: string = bcrypt.hashSync(body.password as string);
        const nonProfitOrganization: nonProfitOrganization = {
            id: NON_EXISTING_ID,
            userTypeId: body.userTypeId,
            login: body.login,
            password: hashedPasword,
            user_id: NON_EXISTING_ID,
            name: body.name,
            email: body.email,
            website: body.website,

        };

        if (nonProfitOrganization.userTypeId !== UserType.nonProfitOrganization) {
            return ResponseHelper.handleError(res, ErrorService.getError(AppError.InputParameterNotSupplied));
        }

        UserService.addUser(nonProfitOrganization, SYSTEM_ID)
            .then((id: number) => {
                nonProfitOrganization.user_id = id;
                return UserService.addNonProfitOrganization(nonProfitOrganization);
            })
            .then(() => {
                return res.status(200).json(nonProfitOrganization.user_id);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            })
    }
    
    
}

export default new UserController();