<template>
    <dialog :open="show">
        <article>
            <header>
                <button aria-label="Close" rel="prev" @click="cancel"></button>
                Create Wallet
            </header>
            <form @submit.prevent="createWallet">
                <input type="text" v-model="walletName" required/>

                <label for="currency">Currency</label>
                <select v-model="currencyId" id="currency" required>
                    <option v-for="currency in currencies" :key="currency.id" :value="currency.id">
                        {{ currency.name }}
                    </option>
                </select>

                <button type="submit">Create</button>
            </form>
        </article>
    </dialog>
</template>

<script lang="ts">
import type { Currency } from '@/data_access/models/currency';


export default {
    props: {
        show: Boolean,
        currencies: Array<Currency>
    },
    data() {
        return {
            walletName: '',
            currencyId: 0 as number
        }
    },
    methods: {
        async createWallet() {
            this.$emit('created', this.walletName, this.currencyId);
            this.walletName = '';
        },
        cancel() {
            this.$emit('cancel');
        }
    }
}
</script>