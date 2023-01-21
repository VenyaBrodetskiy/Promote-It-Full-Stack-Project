export class States {
    private static _endpointBase: string = "http://localhost:4200"
    //all
    public static login: string = "login";
    public static signUp: string = "signup";

    //business owner
    public static campaigns: string = "campaigns";
    public static donateToCampaign: string = "donate-to-campaign";
    public static donateNewProduct: string = "donate-a-product";
    public static orders: string = "orders";
    public static changeState: string = "change-order-status";

    //non-profit organization
    public static createCampaign: string = "create-campaign";
    public static npCampaigns: string = "np-campaigns";

    //social activist
    public static balance: string = "get-balance";
    public static saCampaigns: string = "sa-campaigns";

}

export class Endpoints {
    //all
    private static baseUrlNode: string = "http://localhost:6060/api/"
    private static baseUrlC: string = "https://localhost:7121/api/"

    public static login: string = `${Endpoints.baseUrlNode}auth/login`;
    public static userTypeId: string = `${Endpoints.baseUrlNode}auth/user-type`;

    //business owner
    public static campaigns: string = `${Endpoints.baseUrlNode}campaign/`;
    public static donateNewProduct: string = `${Endpoints.baseUrlC}BusinessOwner/AddProduct`;
    public static donateProductToCampaign: string = `${Endpoints.baseUrlC}BusinessOwner/DonateProductsToCampaign`;
    public static orders: string = `${Endpoints.baseUrlC}BusinessOwner/GetOrders`;
    public static changeState: string = `${Endpoints.baseUrlC}BusinessOwner/ChangeTransactionState`;
    public static products: string = `${Endpoints.baseUrlC}BusinessOwner/GetProducts`;


    //non-profit organization
    public static createCampaign: string = `${Endpoints.baseUrlNode}campaign/`;
    public static npCampaigns: string = `${Endpoints.baseUrlNode}campaign/non-profit/`;

    //social activist
    public static balance: string = "get-balance";
    public static createTransaction: string = `${Endpoints.baseUrlC}SocialActivist/CreateTransaction`;
    public static getProductsForCampaign: string = `${Endpoints.baseUrlNode}campaign/get-all/`;
    public static getBalanceByCampaignId: string = `${Endpoints.baseUrlC}SocialActivist/GetBalanceByCampaignId/`;

    //not used
    public static userById: string = `${Endpoints.baseUrlNode}user/`;

    //add user
    public static addBusinessOwner: string = `${Endpoints.baseUrlNode}user/add-business-owner`;
    public static addSocialActivist: string = `${Endpoints.baseUrlNode}user/add-social-activist`;
    public static addNonProfitUser: string = `${Endpoints.baseUrlNode}user/add-nonprofit-organization`;
}
