import jwt, { JwtPayload } from "jsonwebtoken";
import { Request, Response, NextFunction } from "express";
import { UserType } from "../../common/enums";
import { AuthenticatedRequest } from "../../common/entities";
import { Environment } from "../helpers/env.helper";

interface jwtBase {
    userId: number;
    userTypeId: UserType;
    exp: number;
    iat: number;
}

class AuthMiddleware {

    public verifyToken = (userTypes: UserType[]) => (req: Request, res: Response, next: NextFunction) => {
        let token: string | undefined = req.headers["authorization"]?.toString();

        if (!token) {
            return res.status(403).send("A token is required for authentication");
        }

        try {
            token = token.substring("Bearer ".length);
            const decoded: string | JwtPayload = jwt.verify(token, Environment.TOKEN_SECRET);

            if (userTypes.indexOf((decoded as jwtBase).userTypeId) === -1) {
                return res.sendStatus(401);
            }
            (req as AuthenticatedRequest).userId = (decoded as jwtBase).userId;
            (req as AuthenticatedRequest).userTypeId = (decoded as jwtBase).userTypeId;
        }
        catch (err) {
            return res.status(401).send("Invalid Token");
        }
        return next();
    }

}

export default new AuthMiddleware();