<template>
    <table>
        <thead>
            <tr>
                <th>Id</th>
                <th>Name</th>
                <th>Currency</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="wallet in wallets" :key="wallet.id" @click="updateWallet(wallet.id)">
                <td>{{ wallet.id }}</td>
                <td>{{ wallet.name }}</td>
                <td>{{ wallet.currency.name }}</td>
                <td>
                    <div class="grid">
                        <button class="delete" @click="removeWallet(wallet.id)">
                            Delete
                        </button>
                    </div>
                </td>
            </tr>
        </tbody>
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