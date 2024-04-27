<template>
    <nav>
        <router-link to="/">Dashobards</router-link> |
        <router-link to="/payees">Payees</router-link> |
        <router-link to="/categories">Categories</router-link> |
        <router-link to="/transactions">Transactions</router-link> |
        <!-- <a v-for="wallet in wallets" :key="wallet.id" href="#">{{ wallet.name }}</a> | -->
        <router-link to="/settings">Settings</router-link> |
        <a href="#">Log out</a> |
    </nav>
</template>

<script lang="ts">
import type { Wallet } from '../../data_access/models/wallet'

export default {
    props: {
        wallets: Array<Wallet>
    },
    data() {
        
    }
}
</script>

<style>
</style>