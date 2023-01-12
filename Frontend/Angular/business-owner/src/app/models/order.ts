export interface IOrder {
  id: number;
  userId: number;
  twitterHandle: string;
  email: string;
  address: string;
  phoneNumber: string;
  productId: number;
  productTitle: string;
  productOwnerId: number;
  transactionState: string;
}
