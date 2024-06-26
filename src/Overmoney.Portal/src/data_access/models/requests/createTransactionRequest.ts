export type createTransactionRequest = {
    userId: number;
    walletId: number;
    payeeId: number;
    categoryId: number;
    transactionDate: Date;
    transactionType: number;
    note?: string;
    amount: number;
}