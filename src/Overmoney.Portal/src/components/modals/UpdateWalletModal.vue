<template>
    <dialog :open="show">
        <article>
            <header>
                <button aria-label="Close" rel="prev" @click="cancel"></button>
                Update Wallet
            </header>
            <form @submit.prevent="updateWallet">
                <input type="text" v-model="walletName" required />

                <label for="currency">Currency</label>
                <select v-model="currencyId" id="currency" required>
                    <option v-for="currency in currencies" :key="currency.id" :value="currency.id">
                        {{ currency.name }}
                    </option>
                </select>

                <button type="submit">Update</button>
                <input type="button" class="delete" value="Delete" @click="removeWallet()" :disabled="disableRemove"/>
            </form>
        </article>
    </dialog>
</template>
<script lang="ts">
import type { Currency } from '@/data_access/models/currency';
import type { Wallet } from '../../data_access/models/wallet';
import type { PropType } from 'vue';

export default {
    props: {
        show: Boolean,
        disableRemove: Boolean,
        currencies: Array<Currency>,
        currentValue: {
            type: Object as PropType<Wallet>
        }
    },
    data() {
        return {
            walletName: this.currentValue?.name,
            currencyId: this.currentValue?.currency?.id
        }
    },
    watch: {
        currentValue: function (newValue: Wallet) {
            this.walletName = newValue.name;
            this.currencyId = newValue.currency?.id
        }
    },
    methods: {
        updateWallet() {
            this.$emit('updated', this.currentValue, this.walletName, this.currencyId);
            this.walletName = '';
            this.currencyId = 0;
        },
        cancel() {
            this.$emit('cancel');
        },
        async removeWallet() {
            this.$emit('removeWallet', this.currentValue?.id);
            this.$emit('cancel');
        }
    }
}
</script>