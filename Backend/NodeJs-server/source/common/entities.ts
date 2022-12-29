import { AppError, Role, UserType } from "./enums";
import { Request } from "express";
export interface entityWithId{
    id: number;
}

export interface campaign extends entityWithId {
    hashtag: string;
    landing_page: string;
    name?: string;
}

export interface campaignsWitnProducts extends campaign {
    non_profit_organization_name: string;
    product_title: string;
    business_owner_name: string;
    product_qty: number;

}

export interface jwtUserData {
    userId: number;
    userTypeId: UserType;
    rolesId?: Role[];
}

export interface user extends entityWithId {
    firstName: string;
    lastName: string;
    roles: string[];
    login?: string;
    password?: string;
  
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

