export type updateTransactionRequest = {
    id: number;
    walletId: number;
    payeeId: number;
    categoryId: number;
    transactionDate: Date;
    transactionType: number;
    note?: string;
    amount: number;
}