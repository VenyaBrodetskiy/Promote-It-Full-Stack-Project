export class States {
    private static _endpointBase: string = "http://localhost:4200"
    //all
    public static login: string = "login";

    //business owner
    public static campaigns: string = "campaigns";
    public static donateToCampaign: string = "donate-to-campaign";
    public static donateNewProduct: string = "donate-a-product";
    public static orders: string = "orders";
    public static changeState: string = "change-order-status";

    //non-profit organization
    public static createCampaign: string = "create-campaign";
    public static npCampaigns: string = "np-campaigns";

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


    //TODO: remove?
    public static userById: string = `${Endpoints.baseUrlNode}user/`;

}
