<template>
    <table>
        <tr>
            <th>Id</th>
            <th>Payee</th>
            <th>Category</th>
            <th>Date</th>
            <th>Type</th>
            <th>Amount</th>
            <th>Note</th>
            <th>Attachments?</th>
        </tr>
        <tr v-for="transaction in transactions" :key="transaction.id">
            <td>{{ transaction.id }}</td>
            <td>{{ transaction.payee.name }}</td>
            <td>{{ transaction.category.name }}</td>
            <td>{{ transaction.transactionDate }}</td>
            <td>{{ transaction.transactionType }}</td>
            <td>{{ transaction.amount }}</td>
            <td>{{ transaction.note }}</td>
            <td>{{ transaction.attachments?.length == 0 ? "Yes" : "No" }}</td>
        </tr>
    </table>
</template>

<script lang="ts">
import type {Transaction} from '../../data_access/models/transaction'

export default {
    props: {
        transactions: Array<Transaction>
    }
}
</script>

<style scoped>
table, th, td {
    border: 1px solid pink
}
</style>