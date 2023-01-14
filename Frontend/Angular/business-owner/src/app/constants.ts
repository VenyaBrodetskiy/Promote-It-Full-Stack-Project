export class States {
    private static _endpointBase: string = "http://localhost:4200"
    public static campaigns: string = "campaigns";
    public static donateToCampaign: string = "donate-to-campaign";
    public static donateNewProduct: string = "donate-a-product";
    public static orders: string = "orders";
    public static changeState: string = "change-order-status";
    public static login: string = "login";

}

export class Endpoints {
    private static baseUrlNode: string = "http://localhost:6060/api/"
    private static baseUrlC: string = "https://localhost:7121/api/"

    public static campaigns: string = `${Endpoints.baseUrlNode}campaign/`;

    public static donateNewProduct: string = `${Endpoints.baseUrlC}BusinessOwner/AddProduct`;
    public static donateProductToCampaign: string = `${Endpoints.baseUrlC}BusinessOwner/DonateProductsToCampaign`;
    public static orders: string = `${Endpoints.baseUrlC}BusinessOwner/GetOrders`;
    public static changeState: string = `${Endpoints.baseUrlC}BusinessOwner/ChangeTransactionState`;
    public static products: string = `${Endpoints.baseUrlC}BusinessOwner/GetProducts`;

    public static login: string = `${Endpoints.baseUrlNode}auth/login`;
    public static userTypeId: string = `${Endpoints.baseUrlNode}auth/user-type`;

    public static userById: string = `${Endpoints.baseUrlNode}user/`;

}
