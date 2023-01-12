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
  private static baseUrlNode: string = "http://localhost:6060/"
  private static baseUrlC: string = "https://localhost:7121/"

  public static campaigns: string = `${Endpoints.baseUrlNode}api/campaign/`;

  public static donateNewProduct: string = `${Endpoints.baseUrlC}api/businessowner/addproduct`;
  public static donateProductToCampaign: string = `${Endpoints.baseUrlC}api/businessowner/donateproductstocampaign`;
  public static orders: string = `${Endpoints.baseUrlC}api/businessowner/getorders`;
  public static changeState: string = `${Endpoints.baseUrlC}api/BusinessOwner/ChangeTransactionState`;

  public static login: string = `${Endpoints.baseUrlNode}auth/login`;
  public static userById: string = `${Endpoints.baseUrlNode}user/`;

}
