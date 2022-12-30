import { AppError, Role, UserType } from "./enums";
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
    login?: string;
    password?: string;
}

export interface userInfo {
    user_id: number;
    twitter_handle?: string;
    name?: string;
    email: string;
    address?: string;
    phone_number?: string;
    website?: string;
}

export interface roleType extends entityWithId {
    roleName: string;
}

export interface storeType extends entityWithId {
    name: string;
    address: string;
    openDate: string;
    scale: string;
}

export interface employeeType extends entityWithId{
    firstName: string;
    lastName: string;
    position: string;
}

export interface employeeOfStore extends employeeType{
    storeName: string;
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

export type RoleType = keyof typeof Role;

