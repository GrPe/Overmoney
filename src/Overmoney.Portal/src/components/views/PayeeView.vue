<template>
    <PayeesList :payees="payess"></PayeesList>
</template>

<script lang="ts">
import PayeesList from '../lists/PayeesList.vue'
import type { Payee } from '../../data_access/models/payee'
import { Client } from '@/data_access/client';

export default {
    data() {
        return {
            payess: [] as Array<Payee>
        };
    },
    mounted() {
        let client = new Client();
        client.getPayees(1)
            .then((x) => this.payess = x)
    },
    components: {
        PayeesList,
    }
};
</script>

<style scoped>
body #app header {
    margin: 0;
    padding: 0;
}

header {
    height: 100vh;
    width: 100vw;
    display: flex;
    align-items: center;
    justify-content: center;
}

.wrapper {
    padding-left: 30px;
}
</style>