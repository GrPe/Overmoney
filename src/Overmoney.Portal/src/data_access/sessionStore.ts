import { defineStore } from "pinia";
import type { UserContext } from "./userContext";
import { AuthClient } from "./authClient";

export const userSessionStore = defineStore("user", {
  state: () => {
    return {
      userContext: null as UserContext | null,
    };
  },
  getters: {
    apiToken(): string | undefined {
      return this.userContext?.token;
    },
    getUserId(): number | undefined {
      return this.userContext?.userId;
    },
    isAuthenticated(): boolean {
      //no token
      if (this.userContext === null || this.userContext === undefined) {
        return false;
      }

      //no token
      if (this.userContext?.token === null) {
        return false;
      }

      //token expired
      if (this.userContext?.expiresOn <= new Date()) {
        return false;
      }

      return true;
    },
  },
  actions: {
    async loginUser(email: string, password: string): Promise<boolean> {
      const client = new AuthClient();
      try {
        const authResponse = await client.loginUser(email, password);

        const profileResponse = await client.getUserProfile(
          authResponse.accessToken
        );

        this.userContext = {
          token: authResponse.accessToken,
          userId: profileResponse.id,
          expiresOn: new Date(
            new Date().getTime() + authResponse.expiresIn * 1000
          ),
        };

        return true;
      } catch (error) {
        console.log(error);
        return false;
      }
    },
    logoutUser() {
      this.$reset();
    },
  },
  persist: true,
});
