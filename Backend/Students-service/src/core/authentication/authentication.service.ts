import bcrypt from 'bcryptjs';
import { Queries } from '../../constants';
import { entityWithId, jwtUserData, systemError } from '../../entities';
import { AppError, Role } from '../../enums';
import { SqlHelper } from '../sql.helper';
import ErrorService from "../../core/error.service";

interface localUser extends entityWithId{
    password: string;
    role_id: Role;
}

interface IAuthenticationService {
    login(login: string, password: string):Promise<jwtUserData>;
}

class AuthenticationService implements IAuthenticationService {
    
    constructor() {
    }

    public login(login: string, password: string):Promise<jwtUserData> {
        return new Promise<jwtUserData>((resolve, reject) => {
            SqlHelper.executeQuerySingleResult<localUser>(Queries.GetUserByLogin, login)
            .then((user: localUser) => {
                if (bcrypt.compareSync(password, user.password)) {
                    const result: jwtUserData = {
                        userId: user.id,
                        roleId: user.role_id
                    }
                    resolve(result);
                } 
                else {
                    reject(ErrorService.getError(AppError.NoData));
                }
            })
            .catch((error: systemError) => {
                reject(error);
            });
        }) 
    }

}

export default new AuthenticationService();