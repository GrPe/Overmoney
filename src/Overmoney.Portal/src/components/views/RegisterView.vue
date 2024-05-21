<template>
    <form @submit.prevent="onRegister">
        <input type="text" v-model="email" />
        <input type="password" v-model="password" />
        <input type="confirmPassword" v-model="confirmPassword"/>
        <button type="submit">Login</button>
    </form>
    <p>
        <router-link to="/login">Already have account? Login here!</router-link>
    </p>
</template>

<script lang="ts">
import { AuthClient } from '@/data_access/authClient';


export default {
    data() {
        const authClient = new AuthClient();

        return {
            email: "" as string,
            password: "" as string,
            confirmPassword: "" as string,
            authClient
        }
    },
    methods: {
        async onRegister() {
            if(this.email == "" ) {
                alert("Email cannot be empty!");
                return;
            }
            if(this.password == "" || this.password !== this.confirmPassword) {
                alert("Invalid password");
                return;
            }

            let response = await this.authClient.registerUser(this.email, this.password);

            if(response !== 200) {
                alert("okn't");
                return;
            }

            let profile = await this.authClient.createUserProfile(this.email);

            if(profile !== null && profile !== undefined) {
                alert("ok");
                this.$router.push('/login');
                return;
            }

            alert("okn't 2");
        }
    }
}
</script>