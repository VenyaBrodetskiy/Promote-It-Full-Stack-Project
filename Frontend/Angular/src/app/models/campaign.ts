export interface ICampaign {
    id: number;
    hashtag: string;
    landingPage: string;
    nonProfitOrganizationName?: string;
    title?: string;
}

export interface INewCampaign {
    hashtag: string;
    landingPage: string;
}
