export interface ICampaign {
  id: number;
  hashtag: string;
  landingPage: string;
  nonProfitOrganizationName?: string;
  title?: string;
}

// TODO: remove?
export interface INewCampaign {
  id?: number;
  hashtag: string;
  landingPage: string;
  nonProfitOrganizationName?: string;
  title?: string;
}
