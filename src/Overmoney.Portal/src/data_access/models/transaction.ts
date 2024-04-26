import type { Attachment } from "./attachment";
import type { Category } from "./category";
import type { Payee } from "./payee";
import type { Wallet } from "./wallet";

export type Transaction = {
    id: number;
    userId: number;
    wallet: Wallet;
    payee: Payee;
    category: Category;
    transactionDate: Date;
    transactionType: TransactionType;
    note?: string;
    amount: number;
    attachments?: Array<Attachment>;
}

export enum TransactionType {
    Outcome = 0,
    Income = 1,
    Transfer = 2
}