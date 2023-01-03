import { AppError, UserType } from "./enums";
import { Request } from "express";
export interface entityWithId{
    id: number;
}

export interface campaign extends entityWithId {
    hashtag: string;
    landingPage: string;
    nonProfitOrganizationName?: string;
}

export interface campaignWitnProducts extends campaign {
    productTitle: string;
    businessOwnerName: string;
    productQty: number;

}

export interface jwtUserData {
    userId: number;
    userTypeId: UserType;
}

export interface user extends entityWithId {
    userTypeId: number;
    login: string;
    password: string;
}

export interface socialActivist extends user {
    twitterHandle: string;
    email: string;
    address: string;
    phoneNumber: string;
}

export interface businessOwner extends user {
    twitterHandle: string;
    name: string;
    email: string;
}

export interface nonProfitOrganization extends user {
    name: string;
    email: string;
    website: string;
}

export interface systemError {
    key: AppError
    code: number;
    message: string;
}

export interface authenticationToken {
    userData: jwtUserData;
}

export interface AuthenticatedRequest extends Request, authenticationToken {}

