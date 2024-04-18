import type { Currency } from "./currency";

export interface Wallet {
    id: number;
    userId: number;
    name: string;
    currency: Currency
  }