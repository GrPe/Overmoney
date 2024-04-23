<template>
    <PayeesList :payees="payess" ></PayeesList>
</template>
  
<script lang="ts">
import PayeesList from '../payees/PayeesList.vue'
import type { Wallet } from '../../data_access/models/wallet'
import type { Payee } from '../../data_access/models/payee'

export default {
data() {
    let wallets: Array<Wallet> = [
    { name: "Test", id: 1, userId: 1, currency: { id: 1, code: "PLN", name: "PLN" } },
    { name: "Wallet 2", id: 2, userId: 1, currency: { id: 1, code: "PLN", name: "PLN" } }
    ];
    let payess: Array<Payee> = [
    { name: "Test Payee", id: 1, userId: 1},
    { name: "Wizard", id: 2, userId: 1},
    { name: "7eleven", id: 3, userId: 1},
    { name: "Druid", id: 4, userId: 1},
    ];

    return {
    wallets,
    payess,
    };
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