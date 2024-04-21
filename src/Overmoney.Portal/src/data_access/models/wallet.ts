import type { Currency } from "./currency";

export type Wallet = {
    id: number;
    userId: number;
    name: string;
    currency: Currency
  }