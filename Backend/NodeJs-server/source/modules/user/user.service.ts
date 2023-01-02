import { entityWithId, systemError, user, userInfo } from '../../common/entities';
import { SqlHelper } from '../../core/helpers/sql.helper';
import _ from 'underscore';
import { AppError, Statuses, UserType } from '../../common/enums';
import { DateHelper } from '../../framework/date.helpers';
import { UserQueries } from './user.queries';
import ErrorService from "../../core/error.service";

//TODO: remove unused interfaces
interface localUser {
    id: number;
    login: string;
    user_type_id: number;
}

interface localAddSocialActivist { 
    user_id: number;
    twitter_handle: string;
    email: string;
    address: string;
    phone_number: string;
}

interface localAddBusinessOwner { 
    user_id: number;
    twitter_handle: string;  
    name: string;
    email: string;
}

interface localAddNonProfitOrganization { 
    user_id: number;
    name: string;
    email: string;
    website: string;
}

interface localUserInfo {
    user_id: number;
    twitter_handle?: string;
    name?: string;
    email: string;
    address?: string;
    phone_number?: string;
    website?: string;
}
interface IUserService {

    addUser(user: user, userId: number): Promise<number>;
    addBusinessOwner(userInfo: userInfo): Promise<userInfo>;
    addSocialActivist(userInfo: userInfo): Promise<userInfo>;
    addNonProfitOrganization(userInfo: userInfo): Promise<userInfo>;
}
class UserService implements IUserService {
    
    constructor() { }

    public addUser(user: user, userID: number): Promise<number> {
        return new Promise<number>((resolve, reject) => {
            const createDate: string = DateHelper.dateToString(new Date());

            SqlHelper.createNew(
                UserQueries.AddUser, user,
                user.login as string, user.password as string, user.userTypeId,
                createDate, createDate,
                userID, userID,
                Statuses.Active)
                .then((result: entityWithId) => {
                    resolve(result.id);
                })
                .catch((error: systemError) => reject(error));
        });
    }

    public addBusinessOwner(userInfo: userInfo): Promise<userInfo> {
        return new Promise<userInfo>((resolve, reject) => {
            const createDate: string = DateHelper.dateToString(new Date());

            SqlHelper.executeQueryNoResult(
                UserQueries.AddBusinessOwner, false,
                userInfo.user_id, userInfo.twitter_handle!,
                userInfo.name!, userInfo.email,
                createDate, createDate,
                userInfo.user_id, userInfo.user_id,
                Statuses.Active)
                .then(() => {
                    resolve(userInfo);
                })
                .catch((error: systemError) => reject(error));
        });
    }

    public addSocialActivist(userInfo: userInfo): Promise<userInfo> {
        return new Promise<userInfo>((resolve, reject) => {
            const createDate: string = DateHelper.dateToString(new Date());

            SqlHelper.executeQueryNoResult(
                UserQueries.AddSocialActivist, false,
                userInfo.user_id, userInfo.twitter_handle!, userInfo.email,
                userInfo.address!, userInfo.phone_number!,
                createDate, createDate,
                userInfo.user_id, userInfo.user_id,
                Statuses.Active)
                .then(() => {
                    resolve(userInfo);
                })
                .catch((error: systemError) => reject(error));
        });
    }

    public addNonProfitOrganization(userInfo: userInfo): Promise<userInfo> {
        return new Promise<userInfo>((resolve, reject) => {
            const createDate: string = DateHelper.dateToString(new Date());

            SqlHelper.executeQueryNoResult(
                UserQueries.AddNonProfitOrganization, false,
                userInfo.user_id, userInfo.name!,
                userInfo.email, userInfo.website!,
                createDate, createDate,
                userInfo.user_id, userInfo.user_id,
                Statuses.Active)
                .then(() => {
                    resolve(userInfo);
                })
                .catch((error: systemError) => reject(error));
        });
    }

}

export default new UserService();