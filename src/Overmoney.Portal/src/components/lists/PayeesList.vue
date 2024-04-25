<template>
    <table>
        <tr>
            <th>Id</th>
            <th>Name</th>
        </tr>
        <tr v-for="payee in payees" :key="payee.id">
            <td>{{ payee.id }}</td>
            <td>{{ payee.name }}</td>
        </tr>
    </table>
</template>

<script lang="ts">
import type {Payee} from '../../data_access/models/payee'

export default {
    props: {
        payees: Array<Payee>
    }
}

</script>

<style scoped>
table, th, td {
    border: 1px solid pink
}
</style>