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
    user_id: number;
    twitter_handle: string;
    email: string;
    address: string;
    phone_number: string;
}

export interface businessOwner extends user {
    user_id: number;
    twitter_handle: string;
    name: string;
    email: string;
}

export interface nonProfitOrganization extends user {
    user_id: number;
    name: string;
    email: string;
    website: string;
}

export interface systemError {
    key: AppError
    code: number;
    message: string;
}

export interface sqlParameter {
    name: string;
    type: any;
    value: string | number;
}

export interface authenticationToken {
    userData: jwtUserData;
}

export interface AuthenticatedRequest extends Request, authenticationToken {}

