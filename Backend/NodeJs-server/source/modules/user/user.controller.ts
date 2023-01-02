import { Request, Response, NextFunction } from "express";
import { RequestHelper } from "../../core/helpers/request.helpers";
import { ResponseHelper } from "../../core/helpers/response.helper";
import { AuthenticatedRequest, systemError, user, userInfo } from "../../common/entities";
import { UserType, AppError } from "../../common/enums";
import UserService from "./user.service";
import ErrorService from "../../core/error.service";
import bcrypt from 'bcryptjs';
import { NON_EXISTING_ID } from "../../common/constants";
import loggerService from "../../core/logger.service";

class UserController {

    constructor() { }
    
    public addUser(req: Request, res: Response, next: NextFunction) {

        loggerService.info(`${req.method} ${req.originalUrl}`);
        
        const body: user = req.body;
        const hashedPasword: string = bcrypt.hashSync(body.password as string);

        const user: user = {
            id: NON_EXISTING_ID,
            userTypeId: body.userTypeId,
            login: body.login,
            password: hashedPasword,
        };

        UserService.addUser(user, user.userTypeId) // (req as AuthenticatedRequest).userData.userId
            .then((result: user) => {
                // it's important to replace returnUser in order to hide password
                const returnUser: user = {
                    id: result.id,
                    userTypeId: body.userTypeId,
                    login: body.login,
                }
                return res.status(200).json(returnUser);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            })

    };

    public addUserInfo(req: Request, res: Response, next: NextFunction) {

        loggerService.info(`${req.method} ${req.originalUrl}`);
        
        const numericParamOrError: number | systemError = RequestHelper.parseNumericInput(req.params.userTypeId);
        // TODO: validation for numericParamOrError is one of the user types??

        let userTypeId: UserType;
        if (typeof numericParamOrError === "number") {
            userTypeId = numericParamOrError;
        }
        else 
        {
            return ResponseHelper.handleError(res, ErrorService.getError(AppError.InputParameterNotSupplied));
        }
        // let userTypeId: UserType = 0;

        // switch (numericParamOrError) {
        //     case 1:
        //         userTypeId = UserType.businessOwner;
        //         break;
        //     case 2:
        //         userTypeId = UserType.socialActivist;
        //         break;
        //     case 3:
        //         userTypeId = UserType.nonProfitOrganization;
        //         break;
        //     default:
        //         return ResponseHelper.handleError(res, ErrorService.getError(AppError.InputParameterNotSupplied));
        // }
        
    
        const body: userInfo = req.body;

        const userInfo: userInfo = {
            user_id: body.user_id,
            twitter_handle: body.twitter_handle,
            name: body.name,
            email: body.email,
            address: body.address,
            phone_number: body.phone_number,
            website: body.website,
        };

        UserService.addUserInfo(userInfo, userTypeId) // (req as AuthenticatedRequest).userData.userId
            .then((userInfo: userInfo) => {
                return res.status(200).json(userInfo);
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error);
            })

    };

    
    // public getAll(req: Request, res: Response, next: NextFunction) {
    //     UserService.getAll()
    //         .then((result: user[]) => {
    //             return res.status(200).json(result);
    //         })
    //         .catch((error: systemError) => {
    //             return ResponseHelper.handleError(res, error);
    //         })
    // }

    // public updateById(req: Request, res: Response, next: NextFunction) {

    //     const numericParamOrError: number | systemError = RequestHelper.parseNumericInput(req.params.id);
    
    //     const body: user = req.body;
    //     let inputParamsSupplied: boolean = RequestHelper.checkInputRoles(body.roles);
    
    //     if (typeof numericParamOrError === "number") {
    //         if (numericParamOrError > 0  && inputParamsSupplied === true) {
    //             const user = {
    //                 id: numericParamOrError,
    //                 firstName: body.firstName,
    //                 lastName: body.lastName,
    //                 roles: body.roles
    //             };
                
    //             UserService.updateById(user, (req as AuthenticatedRequest).userData.userId)
    //                 .then((result: user) => {
    //                     return res.status(200).json(result);
    //                 })
    //                 .catch((error: systemError) => {
    //                     return ResponseHelper.handleError(res, error);
    //                 })
    //         }
    //         else {
    //             return ResponseHelper.handleError(res, ErrorService.getError(AppError.InputParameterNotSupplied));
    //         }
    //     }
    //     else {
    //         return ResponseHelper.handleError(res, numericParamOrError);
    //     }  
    // };
    

    
    
    // public deleteById(req: Request, res: Response, next: NextFunction) {
    //     const numericParamOrError: number | systemError = RequestHelper.parseNumericInput(req.params.id);
    
    //     if (typeof numericParamOrError === "number") {
    //         if (numericParamOrError > 0) {
    //             UserService.deleteById(numericParamOrError, (req as AuthenticatedRequest).userData.userId)
    //                 .then(() => {
    //                     return res.sendStatus(200);
    //                 })
    //                 .catch((error: systemError) => {
    //                     return ResponseHelper.handleError(res, error);
    //                 })
    //         }
    //         else {
    //             // TODO: Error handling
    //         }
    //     }
    //     else {
    //         return ResponseHelper.handleError(res, numericParamOrError);
    //     }
        
    // };
}

export default new UserController();