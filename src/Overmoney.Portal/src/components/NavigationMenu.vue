<template>
    <nav>
        <a href="#">Dashobards</a> |
        <a href="#">Payees</a> |
        <a href="#">Categories</a> |
        <a href="#">Transactions</a> |
        <a v-for="wallet in wallets" :key="wallet.id" href="#">{{ wallet.name }}</a> |
        <a href="#">Settings</a> |
        <a href="#">Log out</a> |
    </nav>
</template>

<script lang="ts">
import type { Wallet } from '../data_access/models/wallet'

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