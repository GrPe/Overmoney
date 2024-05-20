import axios, { isCancel, AxiosError } from "axios";
import type { LoginResponse } from "./models/auth/loginResponse";
import type { UserProfile } from "./models/auth/profile";

export class AuthClient {
  async loginUser(email: string, password: string) {
    const response = await axios.post<LoginResponse>(
      import.meta.env.VITE_API_URL +
        `Identity/login?useCookies=false&useSessionCookies=false`,
      { email, password }
    );
    return response.data;
  }

  async getUserProfile(token: string) {
    const response = await axios.get<UserProfile>(
        import.meta.env.VITE_API_URL + `users/profile`, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });
      return response.data;
  }
}