import { UserType } from "./enums";
import { IsString, IsEmail, IsEnum, IsNotEmpty, Matches, IsUrl } from 'class-validator';


export class BusinessOwnerDto {
    @IsEnum(UserType)
    userTypeId?: UserType;

    @IsString()
    @IsNotEmpty()
    login?: string;

    @IsString()
    @IsNotEmpty()
    password?: string;

    @IsString()
    @IsNotEmpty()
    twitterHandle?: string;

    @IsString()
    @IsNotEmpty()
    name?: string;

    @IsEmail()
    @IsNotEmpty()
    email?: string;
}

export class SocialActivistDTO {
    @IsEnum(UserType)
    userTypeId?: UserType;

    @IsString()
    @IsNotEmpty()
    login?: string;

    @IsString()
    @IsNotEmpty()
    password?: string;

    @IsString()
    @IsNotEmpty()
    twitterHandle?: string;

    @IsEmail()
    @IsNotEmpty()
    email?: string;

    @IsString()
    @IsNotEmpty()
    address?: string;

    @IsString({ message: 'Phone number should be string' })
    @Matches(/^\+\d+$/, { message: 'Phone number should start with + and contain only digits' })
    phoneNumber?: string;

}

export class NonProfitOrganizationDTO {
    @IsEnum(UserType)
    userTypeId?: UserType;

    @IsString()
    @IsNotEmpty()
    login?: string;

    @IsString()
    @IsNotEmpty()
    password?: string;

    @IsString()
    @IsNotEmpty()
    name?: string;

    @IsEmail()
    @IsNotEmpty()
    email?: string;

    @IsUrl()
    @IsNotEmpty()
    website?: string;
}

export class CampaignDTO {
    
    @IsString()
    @IsNotEmpty()
    hashtag?: string;

    @IsUrl()
    @IsNotEmpty()
    landingPage?: string;
}