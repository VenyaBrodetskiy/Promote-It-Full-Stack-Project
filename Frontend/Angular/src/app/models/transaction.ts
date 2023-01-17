import { TransactionStates } from "../enums";

export interface ITransaction {
    productId: number;
    campaignId: number;
    stateId: TransactionStates;
}
