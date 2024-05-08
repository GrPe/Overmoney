<template>
    <table>
        <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Actions</th>
        </tr>
        <tr v-for="payee in payees" :key="payee.id">
            <td>{{ payee.id }}</td>
            <td>{{ payee.name }}</td>
            <td>
                <button @click="updatePayee(payee.id)">
                    Edit
                </button>
                <button @click="removePayee(payee.id)">
                    Delete
                </button>
            </td>
        </tr>
    </table>
</template>

<script lang="ts">
import type { Payee } from '../../data_access/models/payee';

export default {
    props: {
        payees: Array<Payee>
    },
    methods: {
        async removePayee(id: number) {
            this.$emit('removePayee', id);
        },
        async updatePayee(id: number) {
            this.$emit('updatePayee', id);
        }
    }
}
</script>

<style scoped>
table, th, td {
    border: 1px solid pink
}
</style>