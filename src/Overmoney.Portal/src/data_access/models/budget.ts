import type { BudgetLine } from "./budgetLine";

export interface Budget {
    id: number;
    name: string;
    userId: number;
    year: number;
    month: number;
    budgetLines: Array<BudgetLine>
}