import type { Category } from "./category";
import type { Payee } from "./payee";
import type { TransactionType } from "./transaction";
import type { Wallet } from "./wallet";

export interface RecurringTransaction {
    id: number;
    userId: number;
    wallet: Wallet;
    payee: Payee;
    category: Category;
    transactionType: TransactionType;
    note: string;
    amount: number;
    nextOccurrence: Date;
    schedule: string;
}