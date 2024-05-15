<template>
    <transition>
        <div v-if="show" class="modal-mask">
            <div class="modal-container">
                <div class="modal-header">
                    <p name="header">Update Transaction</p>
                </div>

                <div class="modal-body">
                    <form @submit.prevent="updateTransaction">
                        <label for="wallet">Wallet</label>
                        <select v-model="transaction.walletId" id="wallet">
                            <option v-for="wallet in wallets" :key="wallet.id" :value="wallet.id">
                                {{ wallet.name }}
                            </option>
                        </select>

                        <label for="category">Category</label>
                        <select v-model="transaction.categoryId" id="category">
                            <option v-for="category in categories" :key="category.id" :value="category.id">
                                {{ category.name }}
                            </option>
                        </select>
                        
                        <label for="payee">Payee</label>
                        <select v-model="transaction.payeeId" id="payee">
                            <option v-for="payee in payees" :key="payee.id" :value="payee.id">
                                {{ payee.name }}
                            </option>
                        </select>

                        <label for="transactionDate">Date</label>
                        <input type="date" id="transactionDate" v-model="transaction.transactionDate"/>

                        <label for="transactionType">Type</label>
                        <select v-model="transaction.transactionType" id="transactionType">
                            <option :value="0">Outcome</option>
                            <option :value="1">Income</option>
                            <option :value="2">Transfer</option>
                        </select>

                        <label for="amount">Amount</label>
                        <input v-model="transaction.amount" id="amount" type="number" step=".01"/>

                        <label for="note">Note</label>
                        <textarea v-model="transaction.note" id="note" placeholder="note...">
                        </textarea>

                        <button type="submit">Create</button>
                    </form>
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </transition>
</template>

<script lang="ts">
import type { Wallet } from '../../data_access/models/wallet'
import type { Payee } from '../../data_access/models/payee'
import type { Category } from '../../data_access/models/category'
import type { Transaction } from '../../data_access/models/transaction'
import { updateTransactionRequest } from '@/data_access/models/requests/updateTransactionRequest'
import { PropType } from 'vue';

export default {
    props: {
        show: Boolean,
        wallets: Array<Wallet>,
        payees: Array<Payee>,
        categories: Array<Category>,
        currentValue: {
            type: Object as PropType<Transaction>
        }
    },
    data() {
        let transaction = {
            id: this.currentValue?.id,
            walletId: this.currentValue?.wallet?.id,
            payeeId: this.currentValue?.payee?.id,
            categoryId: this.currentValue?.category?.id,
            transactionDate: this.currentValue?.transactionDate,
            transactionType: this.currentValue?.transactionType,
            note: this.currentValue?.note,
            amount: this.currentValue?.amount
        } as updateTransactionRequest;

        return {
            transaction,
        }
    },
    watch: {
        currentValue: function(newValue: Transaction) {
            console.log(this.transaction);
            this.transaction = {
                id: newValue?.id,
                walletId: newValue?.wallet?.id,
                payeeId: newValue?.payee?.id,
                categoryId: newValue?.category?.id,
                transactionDate: newValue?.transactionDate,
                transactionType: newValue?.transactionType,
                note: newValue?.note,
                amount: newValue?.amount
            } as updateTransactionRequest;
        }
    },
    methods: {
        async updateTransaction() {
            this.$emit('updated', this.transaction);
            this.transaction = {};
        }
    }
}
</script>