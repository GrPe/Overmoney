<template>
    <table>
        <tr>
            <th>Id</th>
            <th>Wallet</th>
            <th>Payee</th>
            <th>Category</th>
            <th>Date</th>
            <th>Type</th>
            <th>Amount</th>
            <th>Note</th>
            <th>Attachments?</th>
            <th>Actions.</th>
        </tr>
        <tr v-for="transaction in transactions" :key="transaction.id">
            <td>{{ transaction.id }}</td>
            <td>{{ transaction.wallet.name }}</td>
            <td>{{ transaction.payee.name }}</td>
            <td>{{ transaction.category.name }}</td>
            <td>{{ transaction.transactionDate.toLocaleString() }}</td>
            <td>{{ transaction.transactionType }}</td>
            <td>{{ transaction.amount }}</td>
            <td>{{ transaction.note }}</td>
            <td>{{ transaction.attachments?.length == 0 ? "Yes" : "No" }}</td> 

            <td>
                <button @click="updateTransaction(transaction.id)">
                    Edit
                </button>
                <button @click="removeTransaction(transaction.id)">
                    Delete
                </button>
            </td>
        </tr>
    </table>
</template>

<script lang="ts">
import type {Transaction} from '../../data_access/models/transaction'

export default {
    props: {
        transactions: Array<Transaction>
    },
    methods: {
        async updateTransaction(id: number) {
            this.$emit('updateTransaction', id);
        },
        async removeTransaction(id: number) {
            this.$emit('removeTransaction', id);
        }
    }
}
</script>

<style scoped>
table, th, td {
    border: 1px solid pink
}
</style>