<template>
    <form @submit.prevent="onLogin">
        <input type="text" v-model="email" />
        <input type="password" v-model="password" />
        <button type="submit">Login</button>
    </form>
</template>

<script lang="ts">
import { userSessionStore } from '@/data_access/sessionStore';

export default {
    data() {
        let session = userSessionStore();

        return {
            email: "" as string,
            password: "" as string,
            session
        }
    },
    methods: {
        async onLogin() {
            let response = await this.session.loginUser(this.email, this.password);
            if (response === false) {
                console.log("nope!");
            }
            this.email = "";
            this.password = "";
            this.$router.push('/');
        }
    }
}
</script>