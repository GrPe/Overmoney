import type { Category } from "./category";

export interface BudgetLine {
    id: number;
    category: Category
    amount: number;
}