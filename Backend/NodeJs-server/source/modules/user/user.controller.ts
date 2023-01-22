import { Request, Response, NextFunction } from "express";
import bcrypt from 'bcryptjs';
import { NON_EXISTING_ID, SYSTEM_USER_ID } from "../../common/constants";
import { UserType, AppError } from "../../common/enums";
import { systemError, businessOwner, socialActivist, nonProfitOrganization, businessOwnerDTO, socialActivistDTO, nonProfitOrganizationDTO } from "../../common/entities";
import UserService from "./user.service";
import ErrorService from "../../core/error.service";
import LoggerService from "../../core/logger.service";
import { ResponseHelper } from "../../core/helpers/response.helper";
import { validate } from 'class-validator';
import { BusinessOwnerDto, NonProfitOrganizationDTO, SocialActivistDTO } from "../../common/validationclasses";

class UserController {

    constructor() { }

    public getBusinessOwner(req: Request, res: Response, next: NextFunction) {

        LoggerService.info(`${req.method} ${req.originalUrl}`);

        UserService.getBusinessOwner()
            .then((result: businessOwnerDTO[]) => {
                return res.status(200).json(result);
            })
            .catch((error: systemError) => {
                LoggerService.error(`${req.method} ${req.originalUrl}`);
                return ResponseHelper.handleError(res, error);
            })
    }

    public getSocialActivist(req: Request, res: Response, next: NextFunction) {

        LoggerService.info(`${req.method} ${req.originalUrl}`);

        UserService.getSocialActivist()
            .then((result: socialActivistDTO[]) => {
                return res.status(200).json(result);
            })
            .catch((error: systemError) => {
                LoggerService.error(`${req.method} ${req.originalUrl}`);
                return ResponseHelper.handleError(res, error);
            })
    }

    public getNonProfitOrganization(req: Request, res: Response, next: NextFunction) {

        LoggerService.info(`${req.method} ${req.originalUrl}`);

        UserService.getNonProfitOrganization()
            .then((result: nonProfitOrganizationDTO[]) => {
                return res.status(200).json(result);
            })
            .catch((error: systemError) => {
                LoggerService.error(`${req.method} ${req.originalUrl}`);
                return ResponseHelper.handleError(res, error);
            })
    }

    public async addBusinessOwner(req: Request, res: Response, next: NextFunction) {

        LoggerService.info(`${req.method} ${req.originalUrl}`);

        const businessOwnerDto = new BusinessOwnerDto();
        Object.assign(businessOwnerDto, req.body);

        const errors = await validate(businessOwnerDto);
        if (errors.length > 0) {
            LoggerService.error(`Validation error for ${req.method} ${req.originalUrl}`);
            return res.status(422).json({ errors });
        }

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

    public async addSocialActivist(req: Request, res: Response, next: NextFunction) {

        LoggerService.info(`${req.method} ${req.originalUrl}`);

        const socialActivistDTO = new SocialActivistDTO();
        Object.assign(socialActivistDTO, req.body);

        const errors = await validate(socialActivistDTO);
        if (errors.length > 0) {
            LoggerService.error(`Validation error for ${req.method} ${req.originalUrl}`);
            return res.status(422).json({ errors });
        }

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

    public async addNonProfitOrganization(req: Request, res: Response, next: NextFunction) {

        LoggerService.info(`${req.method} ${req.originalUrl}`);

        const nonProfitOrganizationDTO = new NonProfitOrganizationDTO();
        Object.assign(nonProfitOrganizationDTO, req.body);

        const errors = await validate(nonProfitOrganizationDTO);
        if (errors.length > 0) {
            LoggerService.error(`Validation error for ${req.method} ${req.originalUrl}`);
            return res.status(422).json({ errors });
        }

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