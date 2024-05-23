<template>
    <table class="striped">
        <thead>
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
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="transaction in transactions" :key="transaction.id" @click="updateTransaction(transaction.id)">
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
                    <div class="grid">
                        <button class="delete" @click="removeTransaction(transaction.id)">
                            Delete
                        </button>
                        <button @click="addAttachment(transaction.id)">
                            Add Attachment
                        </button>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</template>

<script lang="ts">
import type { Transaction } from '../../data_access/models/transaction'

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
        },
        async addAttachment(id: number) {
            this.$emit('addAttachment', id)
        }
    }
}
</script>