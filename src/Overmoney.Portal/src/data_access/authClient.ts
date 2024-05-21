import axios, { isCancel, AxiosError } from "axios";
import type { LoginResponse } from "./models/auth/loginResponse";
import type { UserProfile } from "./models/auth/profile";

export class AuthClient {
  async loginUser(email: string, password: string): Promise<LoginResponse> {
    const response = await axios.post<LoginResponse>(
      import.meta.env.VITE_API_URL +
        `Identity/login?useCookies=false&useSessionCookies=false`,
      { email, password }
    );
    return response.data;
  }

  async getUserProfile(token: string): Promise<UserProfile> {
    const response = await axios.get<UserProfile>(
      import.meta.env.VITE_API_URL + `users/profile`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response.data;
  }

  async registerUser(email: string, password: string): Promise<number> {
    const response = await axios.post(
      import.meta.env.VITE_API_URL + `Identity/register`,
      { email, password }
    );
    return response.status;
  }

  async createUserProfile(email: string): Promise<UserProfile> {
    const response = await axios.post<UserProfile>(
      import.meta.env.VITE_API_URL + `users/profile`,
      { email }
    );
    return response.data;
  }
}
