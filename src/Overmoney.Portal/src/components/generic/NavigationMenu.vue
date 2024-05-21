<template>
    <nav v-if="session.isAuthenticated">
        <router-link to="/">Dashobards</router-link> |
        <router-link to="/wallets">Wallets</router-link> |
        <router-link to="/payees">Payees</router-link> |
        <router-link to="/categories">Categories</router-link> |
        <router-link to="/transactions">Transactions</router-link> |
        <!-- <a v-for="wallet in wallets" :key="wallet.id" href="#">{{ wallet.name }}</a> | -->
        <router-link to="/settings">Settings</router-link> |
        <a href="#" @click.prevent="logout">Log out</a> |
    </nav>
</template>

<script lang="ts">
import { userSessionStore } from '@/data_access/sessionStore';
import type { Wallet } from '../../data_access/models/wallet'

export default {
    props: {
        wallets: Array<Wallet>
    },
    data() {
        const session = userSessionStore();

        return {
            session
        }
    },
    methods: {
        logout() {
            this.session.logoutUser();
            this.$router.push('/login');
        }
    }
}
</script>

<style>
</style>