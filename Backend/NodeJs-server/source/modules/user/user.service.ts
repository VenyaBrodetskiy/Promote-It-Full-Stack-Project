import { entityWithId, systemError, user, userInfo } from '../../common/entities';
import { SqlHelper } from '../../core/helpers/sql.helper';
import _ from 'underscore';
import { Statuses, UserType } from '../../common/enums';
import { DateHelper } from '../../framework/date.helpers';
import { UserQueries } from './user.queries';

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

    addUser(user: user, userId: number): Promise<user>;

}
class UserService implements IUserService {
    
    constructor() { }

    public addUser(user: user, userId: number): Promise<user> {
        return new Promise<user>((resolve, reject) => {
            const createDate: string = DateHelper.dateToString(new Date());
            
            SqlHelper.createNew(
                UserQueries.AddUser, user, 
                user.login as string, user.password as string, user.userTypeId,  
                createDate, createDate, 
                userId, userId, 
                Statuses.Active)
            .then((result: entityWithId) => {
                resolve(result as user);
            })
            .catch((error: systemError) => reject(error));
        });
    }

    public addUserInfo(userInfo: userInfo, userTypeId: UserType): Promise<userInfo> {
        return new Promise<userInfo>((resolve, reject) => {
            const createDate: string = DateHelper.dateToString(new Date());

            switch (userTypeId) {
                case UserType.socialActivist: 
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
                    break;
                case UserType.businessOwner: 
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
                    break;
                case UserType.nonProfitOrganization: 
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
                    break;
                default:
                    // error?
            }


        });
    }

    // public getAll(): Promise<user[]> {
    //     return new Promise<user[]>((resolve, reject) => {
    //         const result: user[] = [];          
            
    //         SqlHelper.executeQueryArrayResult<localUser>(UserQueries.GetAll, Statuses.Active, Statuses.Active, Statuses.Active)
    //         .then((queryResult: localUser[]) => {
    //             queryResult.reduce((prevUser: localUser, currUser: localUser, index: number) => {
    //                 if (prevUser.id === currUser.id) {
    //                     if (typeof prevUser.role_name == 'string') prevUser.role_name = [prevUser.role_name as string];
    //                     prevUser.role_name.push(currUser.role_name as string);
    //                     if (index === queryResult.length - 1) result.push(this.parseUser(prevUser));
    //                     return prevUser;
    //                 }
    //                 else {
    //                     result.push(this.parseUser(prevUser));
    //                     if (index === queryResult.length - 1) result.push(this.parseUser(currUser));
    //                     return currUser;
    //                 }
    //             });
    //             resolve(result);
    //         })
    //         .catch((error: systemError) => reject(error));
    //     });
    // }

    // public updateById(user: user, userId: number): Promise<user> {
    //     return new Promise<user>((resolve, reject) => {
    //         const updateDate: string = DateHelper.dateToString(new Date());

    //         const [AddRolesExtended, roleParams] = this.prepareQueryToAddRoles(user, updateDate, userId);
    //         Promise.all([
    //             // delete old roles of user (table: user_to_role)
    //             SqlHelper.executeQueryNoResult(
    //                 UserQueries.DeleteRolesOfUser, true,
    //                 updateDate, userId,
    //                 Statuses.NotActive,
    //                 user.id, Statuses.Active),
    //             // updates other user property except roles (table: user)
    //             SqlHelper.executeQueryNoResult(
    //                 UserQueries.UpdateUserById, false, 
    //                 user.firstName, user.lastName, 
    //                 updateDate, userId, 
    //                 user.id, Statuses.Active)
    //         ])
    //         .then(() => {
    //             // adds new roles to user (table: user_to_role)
    //             return SqlHelper.executeQueryNoResult(
    //                 AddRolesExtended as string, false, 
    //                 ...roleParams, 
    //                 );
    //         })
    //         .then(() => {
    //             resolve(user);
    //         })
    //         .catch((error: systemError) => reject(error));
    //     });
    // }

    

    // public deleteById(id: number, userId: number): Promise<void> {
    //     return new Promise<void>((resolve, reject) => {
    //         const updateDate: string = DateHelper.dateToString(new Date());

    //         // TODO: revise this const temp user to passed from request (by auth)
    //         Promise.all([
    //             SqlHelper.executeQueryNoResult(
    //                 UserQueries.DeleteUserById, true, 
    //                 updateDate, userId, 
    //                 Statuses.NotActive, 
    //                 id, Statuses.Active),
    //             SqlHelper.executeQueryNoResult(
    //                 UserQueries.DeleteRolesOfUser, true,
    //                 updateDate, userId,
    //                 Statuses.NotActive,
    //                 id, Statuses.Active
    //             )
    //         ])
    //         .then(() => {
    //             resolve();
    //         })
    //         .catch((error: systemError) => reject(error));

            
    //     });
    // }

    // private parseUser(localUser: localUser): user {
    //     if (typeof localUser.role_name === 'string') localUser.role_name = [localUser.role_name];
    //     return {
    //         id: localUser.id,
    //         firstName: localUser.first_name,
    //         lastName: localUser.last_name,
    //         login: localUser.login,
    //         roles: localUser.role_name
    //     }
    // }

    // private prepareQueryToAddRoles(user: user, createDate: string, userId: number): (string | (string | number)[])[] {
        
    //     const roles: string[] = user.roles;
    //     const params: (string | number)[] = [
    //         user.id, Role[(roles[0] as RoleType)], 
    //         createDate, createDate,
    //         userId, userId, Statuses.Active];
    //     let AddRolesExtended: string = UserQueries.AddRolesToUser;

    //     for (let index = 1; index < roles.length; index++) {
    //         params.push(
    //             user.id, Role[(roles[index] as RoleType)], 
    //             createDate, createDate,
    //             userId, userId, Statuses.Active);
    //         AddRolesExtended += ', (?, ?, ?, ?, ?, ?, ?)'
    //     }

    //     return [AddRolesExtended, params];
    // }
}

export default new UserService();