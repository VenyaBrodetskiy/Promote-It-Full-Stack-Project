import { Request, Response, NextFunction } from "express";
import bcrypt from 'bcryptjs';
import { NON_EXISTING_ID, SYSTEM_USER_ID } from "../../common/constants";
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
            twitterHandle: body.twitterHandle,
            name: body.name,
            email: body.email,
        };

        if (businessOwner.userTypeId !== UserType.businessOwner) {
            return ResponseHelper.handleError(res, ErrorService.getError(AppError.InputParameterNotSupplied));
        }

        UserService.addUser(businessOwner, SYSTEM_USER_ID) 
            .then((id: number) => {
                businessOwner.id = id;
                return UserService.addBusinessOwner(businessOwner);
            })
            .then(() => {
                return res.status(200).json(businessOwner.id);
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
            twitterHandle: body.twitterHandle,
            email: body.email,
            address: body.address,
            phoneNumber: body.phoneNumber,
        };

        if (socialActivist.userTypeId !== UserType.socialActivist) {
            return ResponseHelper.handleError(res, ErrorService.getError(AppError.InputParameterNotSupplied));
        }

        UserService.addUser(socialActivist, SYSTEM_USER_ID)
            .then((id: number) => {
                socialActivist.id = id;
                return UserService.addSocialActivist(socialActivist);
            })
            .then(() => {
                return res.status(200).json(socialActivist.id);
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
            name: body.name,
            email: body.email,
            website: body.website,

        };

        if (nonProfitOrganization.userTypeId !== UserType.nonProfitOrganization) {
            return ResponseHelper.handleError(res, ErrorService.getError(AppError.InputParameterNotSupplied));
        }

        UserService.addUser(nonProfitOrganization, SYSTEM_USER_ID)
            .then((id: number) => {
                nonProfitOrganization.id = id;
                return UserService.addNonProfitOrganization(nonProfitOrganization);
            })
            .then(() => {
                return res.status(200).json(nonProfitOrganization.id);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            })
    }
    
    
}

export default new UserController();