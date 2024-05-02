import type { Category } from "./models/category";
import axios, {isCancel, AxiosError} from 'axios';
import type { Payee } from "./models/payee";

export class Client {
    async getCategories(userId: number) : Promise<Array<Category>> {
        const response = await axios.get<Array<Category>>(import.meta.env.VITE_API_URL + `users/${userId}/categories`);
        return response.data;
    }

    async getPayees(userId: number) : Promise<Array<Payee>> {
        const response = await axios.get<Array<Payee>>(import.meta.env.VITE_API_URL + `users/${userId}/payees`);
        return response.data;
    }
}