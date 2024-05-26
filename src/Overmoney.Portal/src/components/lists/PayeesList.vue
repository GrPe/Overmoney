<template>
    <table class="striped">
        <thead>
            <tr>
                <th>Id</th>
                <th>Name</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="payee in payees" :key="payee.id" @click="updatePayee(payee.id)">
                <td>{{ payee.id }}</td>
                <td>{{ payee.name }}</td>
            </tr>
        </tbody>
    </table>
</template>

<script lang="ts">
import type { Payee } from '../../data_access/models/payee';

export default {
    props: {
        payees: Array<Payee>
    },
    methods: {
        async updatePayee(id: number) {
            this.$emit('updatePayee', id);
        }
    }
}
</script>