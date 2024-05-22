<template>
    <table class="striped">
        <thead>
            <th>Id</th>
            <th>Name</th>
            <th>Actions</th>
        </thead>
        <tr v-for="payee in payees" :key="payee.id">
            <td>{{ payee.id }}</td>
            <td>{{ payee.name }}</td>
            <td>
                <div class="grid">
                    <button @click="updatePayee(payee.id)">
                        Edit
                    </button>
                    <button class="delete" @click="removePayee(payee.id)">
                        Delete
                    </button>
                </div>
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