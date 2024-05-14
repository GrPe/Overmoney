<template>
    <button @click="showModal = true">Add new</button>
    <TransactionList :transactions="transactions"></TransactionList>
    <CreateTransactionModal :show="showModal" :wallets="wallets" :payees="payees" :categories="categories"
        @created="onCreateTransaction" />
</template>

<script lang="ts">
import TransactionList from '../lists/TransactionList.vue'
import type { Wallet } from '../../data_access/models/wallet'
import type { Payee } from '../../data_access/models/payee';
import type { Transaction } from '../../data_access/models/transaction'
import type { Category } from '../../data_access/models/category';
import type { createTransactionRequest } from '@/data_access/models/requests/createTransactionRequest';
import { Client } from '@/data_access/client';
import type { UserContext } from '@/data_access/userContext';
import CreateTransactionModal from '@/components/modals/CreateTransactionModal.vue';

export default {
    data() {
        const client = new Client();
        return {
            client,
            categories: [] as Array<Category>,
            payees: [] as Array<Payee>,
            transactions: [] as Array<Transaction>,
            wallets: [] as Array<Wallet>,
            showModal: false,
            // showUpdateModal: false,
            // categoryToUpdate: {} as Category | undefined,
            userContext: { userId: 1 } as UserContext
        }

    },
    mounted() {
        this.client.getWallets(this.userContext.userId)
            .then(x => { this.wallets = x })
            .then(() => this.client.getCategories(this.userContext.userId))
            .then(x => { this.categories = x })
            .then(() => this.client.getPayees(this.userContext.userId))
            .then(x => { this.payees = x })
            .then(() => this.client.getTransactions(this.userContext.userId))
            .then(x => { this.transactions = x });
    },
    methods: {
        async onCreateTransaction(transaction: createTransactionRequest) {
            this.showModal = false;
            transaction.userId = this.userContext.userId;
            let result = await this.client.createTransaction(transaction);
            this.transactions.push(result);
        }
    },
    components: {
        TransactionList,
        CreateTransactionModal,
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