<template>
    <div class="center-form">
        <h1>Register</h1>
        <form @submit.prevent="onRegister">
            <input type="text" placeholder="Email" v-model="email" required />
            <input type="password" placeholder="Password" v-model="password" required/>
            <input type="confirmPassword" placeholder="Confirm password" v-model="confirmPassword" required/>
            <button type="submit">Register</button>
        </form>
        <p>
            <router-link to="/login">Already have account? Login here!</router-link>
        </p>
    </div>
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
            if (this.email == "") {
                alert("Email cannot be empty!");
                return;
            }
            if (this.password == "" || this.password !== this.confirmPassword) {
                alert("Invalid password");
                return;
            }

            let response = await this.authClient.registerUser(this.email, this.password);

            if (response !== 200) {
                return;
            }

            let profile = await this.authClient.createUserProfile(this.email);

            if (profile !== null && profile !== undefined) {
                this.$router.push('/login');
                return;
            }
        }
    }
}
</script>