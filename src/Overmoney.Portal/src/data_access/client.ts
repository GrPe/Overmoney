import axios, { isCancel, AxiosError } from "axios";
import type { Category } from "./models/category";
import type { Payee } from "./models/payee";
import type { createCategoryRequest } from "./models/requests/createCategoryRequest";
import type { updateCategoryRequest } from "./models/requests/createCategoryRequest";
import type { createPayeeReqeuest } from "./models/requests/createPayeeReqeuest";
import type { updatePayeeRequest } from "./models/requests/updatePayeeRequest";

export class Client {
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
}
