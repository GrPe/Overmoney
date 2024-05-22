<template>
    <aside v-if="session.isAuthenticated">
        <nav>
            <ul>
                <li><router-link to="/" class="contrast">Dashobards</router-link></li>
                <li><router-link to="/wallets" class="contrast">Wallets</router-link></li>
                <li><router-link to="/payees" class="contrast">Payees</router-link></li>
                <li><router-link to="/categories" class="contrast">Categories</router-link></li>
                <li><router-link to="/transactions" class="contrast">Transactions</router-link></li>
                <li><router-link to="/settings" class="contrast">Settings</router-link></li>
                <li><a href="#" @click.prevent="logout" class="contrast">Log out</a></li>
            </ul>
            <!-- <a v-for="wallet in wallets" :key="wallet.id" href="#">{{ wallet.name }}</a> | -->
        </nav>
    </aside>
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

<style scoped>
nav {
    margin: 0 0 0 5px ;
    padding: 2px;
};
</style>
