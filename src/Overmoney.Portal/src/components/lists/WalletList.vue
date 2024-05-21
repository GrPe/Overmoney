<template>
    <table>
        <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Currency</th>
            <th>Actions</th>
        </tr>
        <tr v-for="wallet in wallets" :key="wallet.id">
            <td>{{ wallet.id }}</td>
            <td>{{ wallet.name }}</td>
            <td>{{ wallet.currency.name }}</td>
            <td>
                <button @click="updateWallet(wallet.id)">
                    Edit
                </button>
                <button @click="removeWallet(wallet.id)">
                    Delete
                </button>
            </td>
        </tr>
    </table>
</template>

<script lang="ts">
import type { Wallet } from '../../data_access/models/wallet';

export default {
    props: {
        wallets: Array<Wallet>
    },
    methods: {
        async updateWallet(id: number) {
            this.$emit('updateWallet', id);
        },
        async removeWallet(id: number) {
            this.$emit('removeWallet', id);
        }
    }
}
</script>

<style scoped>
table, th, td {
    border: 1px solid pink
}
</style>