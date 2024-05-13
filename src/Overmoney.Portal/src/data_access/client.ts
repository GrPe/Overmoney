import axios, { isCancel, AxiosError } from "axios";
import type { Category } from "./models/category";
import type { Payee } from "./models/payee";
import type { createCategoryRequest } from "./models/requests/createCategoryRequest";
import type { updateCategoryRequest } from "./models/requests/createCategoryRequest";
import type { createPayeeReqeuest } from "./models/requests/createPayeeReqeuest";
import type { updatePayeeRequest } from "./models/requests/updatePayeeRequest";
import type { createTransactionRequest } from "./models/requests/createTransactionRequest";
import type { Wallet } from "./models/wallet";
import type { Transaction } from "./models/transaction";

export class Client {
  async getWallets(userId: number): Promise<Array<Wallet>> {
    const response = await axios.get<Array<Wallet>>(
      import.meta.env.VITE_API_URL + `users/${userId}/wallets`
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

  async createTransaction(
    request: createTransactionRequest
  ): Promise<Transaction> {
    const response = await axios.post(
      import.meta.env.VITE_API_URL + "transactions",
      request
    );
    return response.data;
  }
}
