<template>
    <div class="center-form">
        <h1>Sign in</h1>
        <form @submit.prevent="onLogin">
            <input type="text" placeholder="Login" v-model="email" required />
            <input type="password" placeholder="Password" v-model="password" required />
            <button type="submit">Login</button>
        </form>
        <p>
            <router-link to="/register">Register here!</router-link>
        </p>
    </div>
</template>

<script lang="ts">
import { AuthClient } from '@/data_access/authClient';
import { userSessionStore } from '@/data_access/sessionStore';

export default {
    data() {
        let session = userSessionStore();
        const authClient = new AuthClient();

        return {
            email: "" as string,
            password: "" as string,
            session,
            authClient
        }
    },
    methods: {
        async onLogin() {
            let response = await this.session.loginUser(this.email, this.password);
            if (response === false) {
                console.log("nope!")
            }
            this.email = "";
            this.password = "";
            this.$router.push('/');
        }
    }
}
</script>