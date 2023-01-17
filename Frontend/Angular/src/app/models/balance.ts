export interface IBalance {
    id: number;
    twitterHandle?: string;
    campaignHashtag?: string;
    balance: number;
    previousTweetCount?: number;
    updateDate?: string;
}
