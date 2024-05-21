<template>
    <transition>
        <div v-if="show" class="modal-mask">
            <div class="modal-container">
                <div class="modal-header">
                    <p name="header">Create Wallet</p>
                </div>

                <div class="modal-body">
                    <form @submit.prevent="createWallet">
                        <input type="text" v-model="walletName"/>

                        <label for="currency">Currency</label>
                        <select v-model="currencyId" id="currency">
                            <option v-for="currency in currencies" :key="currency.id" :value="currency.id">
                                {{ currency.name }}
                            </option>
                        </select>

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
        }
    }
}
</script>