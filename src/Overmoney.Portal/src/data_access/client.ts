import axios, { isCancel, AxiosError } from "axios";
import type { Category } from "./models/category";
import type { Payee } from "./models/payee";
import type { createCategoryRequest } from "./models/requests/createCategoryRequest";
import type { updateCategoryRequest } from "./models/requests/updateCategoryRequest";
import type { createPayeeReqeuest } from "./models/requests/createPayeeReqeuest";
import type { updatePayeeRequest } from "./models/requests/updatePayeeRequest";
import type { createTransactionRequest } from "./models/requests/createTransactionRequest";
import type { Wallet } from "./models/wallet";
import type { Transaction } from "./models/transaction";
import type { updateTransactionRequest } from "./models/requests/updateTransactionRequest";
import type { createWalletRequest } from "./models/requests/createWalletRequest";
import type { Currency } from "./models/currency";
import type { updateWalletRequest } from "./models/requests/updateWalletRequest";

export class Client {
  async getWallets(userId: number): Promise<Array<Wallet>> {
    const response = await axios.get<Array<Wallet>>(
      import.meta.env.VITE_API_URL + `users/${userId}/wallets`
    );
    return response.data;
  }

  async createWallet(request: createWalletRequest): Promise<Wallet> {
    const response = await axios.post(
      import.meta.env.VITE_API_URL + "wallets",
      request
    );
    return response.data;
  }

  async updateWallet(request: updateWalletRequest): Promise<void> {
    await axios.put(import.meta.env.VITE_API_URL + "wallets", request);
  }

  async removeWallet(walletId: number): Promise<void> {
    await axios.delete(
      import.meta.env.VITE_API_URL + "wallets/" + walletId.toString()
    );
  }

  async getCurrencies(): Promise<Array<Currency>> {
    const response = await axios.get<Array<Currency>>(
      import.meta.env.VITE_API_URL + `currencies`
    );
    return response.data;
  }

  async getCategories(userId: number): Promise<Array<Category>> {
    const response = await axios.get<Array<Category>>(
      import.meta.env.VITE_API_URL + `users/${userId}/categories`
    );
    return response.data;
  }

  async createCategory(request: createCategoryRequest): Promise<Category> {
    const response = await axios.post(
      import.meta.env.VITE_API_URL + "categories",
      request
    );
    return response.data;
  }

  async removeCategory(categoryId: number): Promise<void> {
    await axios.delete(
      import.meta.env.VITE_API_URL + "categories/" + categoryId.toString()
    );
  }

  async updateCategory(request: updateCategoryRequest): Promise<void> {
    await axios.put(import.meta.env.VITE_API_URL + "categories", request);
  }

  async getPayees(userId: number): Promise<Array<Payee>> {
    const response = await axios.get<Array<Payee>>(
      import.meta.env.VITE_API_URL + `users/${userId}/payees`
    );
    return response.data;
  }

  async createPayee(request: createPayeeReqeuest): Promise<Payee> {
    const response = await axios.post(
      import.meta.env.VITE_API_URL + "payees",
      request
    );
    return response.data;
  }

  async removePayee(payeeId: number): Promise<void> {
    await axios.delete(
      import.meta.env.VITE_API_URL + "payees/" + payeeId.toString()
    );
  }

  async updatePayee(request: updatePayeeRequest): Promise<void> {
    await axios.put(import.meta.env.VITE_API_URL + "payees", request);
  }

  async getTransactions(userId: number): Promise<Array<Transaction>> {
    const response = await axios.get<Array<Transaction>>(
      import.meta.env.VITE_API_URL + `users/${userId}/transactions`
    );
    return response.data;
  }

  async getTransactionsByWallet(userId: number, walletId: number): Promise<Transaction[] | null> {
    const response = await axios.get<Array<Transaction>>(
      import.meta.env.VITE_API_URL +
        `users/${userId}/transactions?walletId=${walletId}`
    );

    if (response == null) {
      return null;
    }

    return response.data;
  }

  async getTransactionsByCategory(userId: number, categoryId: number): Promise<Transaction[] | null> {
    const response = await axios.get<Array<Transaction>>(
      import.meta.env.VITE_API_URL +
        `users/${userId}/transactions?categoryId=${categoryId}`
    );

    if (response == null) {
      return null;
    }

    return response.data;
  }

  async getTransactionsByPayee(userId: number, payeeId: number): Promise<Transaction[] | null> {
    const response = await axios.get<Array<Transaction>>(
      import.meta.env.VITE_API_URL +
        `users/${userId}/transactions?payeeId=${payeeId}`
    );

    if (response == null) {
      return null;
    }

    return response.data;
  }

  async createTransaction(
    request: createTransactionRequest
  ): Promise<Transaction> {
    const response = await axios.post(
      import.meta.env.VITE_API_URL + "transactions",
      request
    );
    return response.data;
  }

  async updateTransaction(request: updateTransactionRequest): Promise<void> {
    await axios.put(import.meta.env.VITE_API_URL + "transactions", request);
  }

  async removeTransaction(transactionId: number): Promise<void> {
    await axios.delete(
      import.meta.env.VITE_API_URL + "transactions/" + transactionId.toString()
    );
  }
}
