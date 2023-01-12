import { Request, Response, NextFunction } from "express";
import { authenticationToken, systemError } from "../../common/entities";
import { ResponseHelper } from "../helpers/response.helper";
import AuthenticationService from './authentication.service';
import jwt from 'jsonwebtoken';
import { Environment } from "../helpers/env.helper";


interface localUser {
    login: string;
    password: string;
}

class AuthenticationController {

    constructor() { }

    public login(req: Request, res: Response, next: NextFunction) {
        // we use body (not url) in order 
        const user: localUser = req.body;

        AuthenticationService.login(user.login, user.password)
            .then((userData: authenticationToken) => {
                // TODO: generate JWT token

                const token: string = jwt.sign(
                    userData,
                    Environment.TOKEN_SECRET,
                    {
                        expiresIn: "2h",
                    }
                );

                return res.status(200).json({
                    token: token
                });
                // TODO: handle error
            })
            .catch((error: systemError) => {
                return ResponseHelper.handleError(res, error, true);
            })
    }
}

// creating singleton
export default new AuthenticationController();