import _ from 'underscore';
import { Statuses } from '../../common/enums';
import { entityWithId, systemError, user, businessOwner, socialActivist, nonProfitOrganization, businessOwnerDTO, socialActivistDTO, nonProfitOrganizationDTO } from '../../common/entities';
import { UserQueries } from './user.queries';
import { DateHelper } from '../../framework/date.helpers';
import { SqlHelper } from '../../core/helpers/sql.helper';

interface localBusinessOwner extends entityWithId {
    name: string;
    email: string;
    twitter_handle: string;
}

export interface localSocialActivist extends entityWithId {
    email: string;
    twitter_handle: string;
    address: string;
    phone_number: string;
}

interface localNonProfitOrganization extends entityWithId {
    name: string;
    email: string;
    website: string;
}
interface IUserService {
    getBusinessOwner(): Promise<businessOwnerDTO[]>;
    getSocialActivist(): Promise<socialActivistDTO[]>;
    getNonProfitOrganization(): Promise<nonProfitOrganizationDTO[]>
    addUser(user: user, userId: number): Promise<number>;
    addBusinessOwner(businessOwner: businessOwner): Promise<void>;
    addSocialActivist(socialActivist: socialActivist): Promise<void>;
    addNonProfitOrganization(nonProfitOrganization: nonProfitOrganization): Promise<void>;
}
class UserService implements IUserService {
    
    constructor() { }

    public getBusinessOwner(): Promise<businessOwnerDTO[]> {
        return new Promise<businessOwnerDTO[]>((resolve, reject) => {
            const result: businessOwnerDTO[] = [];

            SqlHelper.executeQueryArrayResult<localBusinessOwner>(UserQueries.GetAllBusinessOwners, Statuses.Active)
                .then((queryResult: localBusinessOwner[]) => {
                    queryResult.forEach((user: localBusinessOwner) => {
                        result.push(this.parseLocalBusinessOwner(user))
                    });
                    resolve(result);
                })
                .catch((error: systemError) => reject(error));
        });
    }

    public getSocialActivist(): Promise<socialActivistDTO[]> {
        return new Promise<socialActivistDTO[]>((resolve, reject) => {
            const result: socialActivistDTO[] = [];

            SqlHelper.executeQueryArrayResult<localSocialActivist>(UserQueries.GetAllSocialActivists, Statuses.Active)
                .then((queryResult: localSocialActivist[]) => {
                    queryResult.forEach((user: localSocialActivist) => {
                        result.push(this.parseLocalSocialActivist(user))
                    });
                    resolve(result);
                })
                .catch((error: systemError) => reject(error));
        });
    }

    public getNonProfitOrganization(): Promise<nonProfitOrganizationDTO[]> {
        return new Promise<nonProfitOrganizationDTO[]>((resolve, reject) => {
            const result: nonProfitOrganizationDTO[] = [];

            SqlHelper.executeQueryArrayResult<localNonProfitOrganization>(UserQueries.GetAllNonProfitOrganizations, Statuses.Active)
                .then((queryResult: localNonProfitOrganization[]) => {
                    queryResult.forEach((user: localNonProfitOrganization) => {
                        result.push(this.parseLocalNonProfitOrganization(user))
                    });
                    resolve(result);
                })
                .catch((error: systemError) => reject(error));
        });
    }

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
                businessOwner.id, businessOwner.twitterHandle,
                businessOwner.name, businessOwner.email,
                createDate, createDate,
                businessOwner.id, businessOwner.id,
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
                socialActivist.id, socialActivist.twitterHandle, socialActivist.email,
                socialActivist.address, socialActivist.phoneNumber,
                createDate, createDate,
                socialActivist.id, socialActivist.id,
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
                nonProfitOrganization.id, nonProfitOrganization.name,
                nonProfitOrganization.email, nonProfitOrganization.website,
                createDate, createDate,
                nonProfitOrganization.id, nonProfitOrganization.id,
                Statuses.Active)
                .then(() => {
                    resolve();
                })
                .catch((error: systemError) => reject(error));
        });
    }

    private parseLocalBusinessOwner(user: localBusinessOwner): businessOwnerDTO {
        return {
            id: user.id,
            name: user.name,
            email: user.email,
            twitterHandle: user.twitter_handle
        }
    }

    private parseLocalSocialActivist(user: localSocialActivist): socialActivistDTO {
        return {
            id: user.id,
            address: user.address,
            phoneNumber: user.phone_number,
            email: user.email,
            twitterHandle: user.twitter_handle
        }
    }

    private parseLocalNonProfitOrganization(user: localNonProfitOrganization): nonProfitOrganizationDTO {
        return {
            id: user.id,
            name: user.name,
            email: user.email,
            website: user.website
        }
    }

}

export default new UserService();