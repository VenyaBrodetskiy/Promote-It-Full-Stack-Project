export interface IUser {
    login: string;
    password: string;
}

export interface IBusinessOwner extends IUser {
    userTypeId: number,
    twitterHandle: string,
    name: string,
    email: string
}

export interface ISocialActivist extends IUser {
    userTypeId: number,
    twitterHandle: string,
    email: string,
    address: string,
    phoneNumber: string
}

export interface INonProfitOrg extends IUser {
    userTypeId: number,
    name: string,
    email: string,
    website: string
}
