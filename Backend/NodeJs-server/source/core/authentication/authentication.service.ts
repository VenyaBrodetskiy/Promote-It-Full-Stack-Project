import bcrypt from 'bcryptjs';
import { entityWithId, jwtUserData, systemError } from '../../common/entities';
import { AppError, Statuses, UserType } from '../../common/enums';
import { SqlHelper } from '../helpers/sql.helper';
import ErrorService from "../../core/error.service";
import { AuthenticationQueries } from './authentication.queries';

interface  localUser extends entityWithId{
    password: string;
    user_type_id: UserType;
}

interface IAuthenticationService {
    login(login: string, password: string):Promise<jwtUserData>;
}

export class AuthenticationService implements IAuthenticationService {
    
    constructor() {
    }

    public login(login: string, password: string): Promise<jwtUserData> {

        return new Promise<jwtUserData>((resolve, reject) => {
            SqlHelper.executeQuerySingleResult<localUser>(
                AuthenticationQueries.GetUserByLogin, login, 
                Statuses.Active, Statuses.Active)
            .then((user: localUser) => {
                if (bcrypt.compareSync(password, user.password)) {
                    const result: jwtUserData = {
                        userId: user.id,
                        userTypeId: user.user_type_id
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

    // private static createPassword() {
        // const temp_pass: string = bcrypt.hashSync('password');
        // console.log(temp_pass);
        // console.log(bcrypt.compareSync('password', '$2a$10$nk.AB39zLdPrYLhtyP6o7u93Vk7SmGVTYqgMhh8l6YlQjb8xaLU9u'));
    //}
    
}

export default new AuthenticationService();