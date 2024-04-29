import type { Category } from "./models/category";
import axios, {isCancel, AxiosError} from 'axios';

export class Client {
    async getCategories(userId: number) : Promise<Array<Category> | null> {
        const response = await axios.get(import.meta.env.VITE_API_URL + `/users/${userId}/categories`);
        console.log(response);
        return null;
    }
}