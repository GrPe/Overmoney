<template>
    <TransactionList :transactions="transactions" ></TransactionList>
    
</template>
  
<script lang="ts">
import TransactionList from '../lists/TransactionList.vue'
import type { Wallet } from '../../data_access/models/wallet'
import type { Payee } from '../../data_access/models/payee';
import type { Transaction } from '../../data_access/models/transaction'
import type { Category } from '../../data_access/models/category';

export default {
data() {
    let wallets: Array<Wallet> = [
        { name: "Test", id: 1, userId: 1, currency: { id: 1, code: "PLN", name: "PLN" } },
        { name: "Wallet 2", id: 2, userId: 1, currency: { id: 1, code: "PLN", name: "PLN" } }
    ];
    let payees: Array<Payee> = [
        { name: "Test Payee", id: 1, userId: 1},
        { name: "Wizard", id: 2, userId: 1},
        { name: "7eleven", id: 3, userId: 1},
        { name: "Druid", id: 4, userId: 1},
    ];
    let categories: Array<Category> = [
        { name: "Test Category", id: 1, userId: 1},
        { name: "Food", id: 2, userId: 1},
        { name: "Home", id: 3, userId: 1},
        { name: "Horse", id: 4, userId: 1},
    ];

    let transactions: Array<Transaction> = [
        { id: 1, userId: 1, wallet: wallets[0], payee: payees[0], category: categories[0], transactionDate: new Date(), transactionType: 0, note: "Lorem, ipsum.", amount: 526.4 },
        { id: 2, userId: 1, wallet: wallets[0], payee: payees[1], category: categories[0], transactionDate: new Date(), transactionType: 0, note: "Lorem ipsum dolor sit.", amount: 1.4 },
        { id: 3, userId: 1, wallet: wallets[0], payee: payees[1], category: categories[0], transactionDate: new Date(), transactionType: 0, note: "", amount: 132.47 },
        { id: 4, userId: 1, wallet: wallets[0], payee: payees[2], category: categories[1], transactionDate: new Date(), transactionType: 0, note: "Lorem ipsum dolor sit.", amount: 185.40 },
        { id: 5, userId: 1, wallet: wallets[0], payee: payees[2], category: categories[2], transactionDate: new Date(), transactionType: 1, note: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Nihil libero eos esse sapiente labore delectus vero facere totam suscipit sint!", amount: 132.4 },
    ]

    return {
        wallets,
        transactions
    };
},
components: {
    TransactionList,
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