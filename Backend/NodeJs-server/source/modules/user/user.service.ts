import _ from 'underscore';
import { Statuses } from '../../common/enums';
import { entityWithId, systemError, user, businessOwner, socialActivist, nonProfitOrganization } from '../../common/entities';
import { UserQueries } from './user.queries';
import { DateHelper } from '../../framework/date.helpers';
import { SqlHelper } from '../../core/helpers/sql.helper';

interface IUserService {

    addUser(user: user, userId: number): Promise<number>;
    addBusinessOwner(businessOwner: businessOwner): Promise<void>;
    addSocialActivist(socialActivist: socialActivist): Promise<void>;
    addNonProfitOrganization(nonProfitOrganization: nonProfitOrganization): Promise<void>;
}
class UserService implements IUserService {
    
    constructor() { }

    public addUser(user: user, createUserId: number): Promise<number> {
        return new Promise<number>((resolve, reject) => {
            const createDate: string = DateHelper.dateToString(new Date());

            SqlHelper.createNew(
                UserQueries.AddUser, user,
                user.login as string, user.password as string, user.userTypeId,
                createDate, createDate,
                createUserId, createUserId,
                Statuses.Active)
                .then((result: entityWithId) => {
                    resolve(result.id);
                })
                .catch((error: systemError) => reject(error));
        });
    }

    public addBusinessOwner(businessOwner: businessOwner): Promise<void> {
        return new Promise<void>((resolve, reject) => {
            const createDate: string = DateHelper.dateToString(new Date());

            SqlHelper.executeQueryNoResult(
                UserQueries.AddBusinessOwner, false,
                businessOwner.user_id, businessOwner.twitter_handle,
                businessOwner.name, businessOwner.email,
                createDate, createDate,
                businessOwner.user_id, businessOwner.user_id,
                Statuses.Active)
                .then(() => {
                    resolve();
                })
                .catch((error: systemError) => reject(error));
        });
    }

    public addSocialActivist(socialActivist: socialActivist): Promise<void> {
        return new Promise<void>((resolve, reject) => {
            const createDate: string = DateHelper.dateToString(new Date());

            SqlHelper.executeQueryNoResult(
                UserQueries.AddSocialActivist, false,
                socialActivist.user_id, socialActivist.twitter_handle, socialActivist.email,
                socialActivist.address, socialActivist.phone_number,
                createDate, createDate,
                socialActivist.user_id, socialActivist.user_id,
                Statuses.Active)
                .then(() => {
                    resolve();
                })
                .catch((error: systemError) => reject(error));
        });
    }

    public addNonProfitOrganization(nonProfitOrganization: nonProfitOrganization): Promise<void> {
        return new Promise<void>((resolve, reject) => {
            const createDate: string = DateHelper.dateToString(new Date());

            SqlHelper.executeQueryNoResult(
                UserQueries.AddNonProfitOrganization, false,
                nonProfitOrganization.user_id, nonProfitOrganization.name,
                nonProfitOrganization.email, nonProfitOrganization.website,
                createDate, createDate,
                nonProfitOrganization.user_id, nonProfitOrganization.user_id,
                Statuses.Active)
                .then(() => {
                    resolve();
                })
                .catch((error: systemError) => reject(error));
        });
    }

}

export default new UserService();