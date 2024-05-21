<template>
    <transition>
        <div v-if="show" class="modal-mask">
            <div class="modal-container">
                <div class="modal-header">
                    <p name="header">Update Wallet</p>
                </div>

                <div class="modal-body">
                    <form @submit.prevent="updateWallet">
                        <input type="text" v-model="walletName"/>

                        <label for="currency">Currency</label>
                        <select v-model="currencyId" id="currency">
                            <option v-for="currency in currencies" :key="currency.id" :value="currency.id">
                                {{ currency.name }}
                            </option>
                        </select>

                        <button type="submit">Update</button>
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
import type { Wallet } from '../../data_access/models/wallet';
import type { PropType } from 'vue';

export default {
    props: {
        show: Boolean,
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
        currentValue: function(newValue : Wallet) {
            this.walletName = newValue.name;
            this.currencyId = newValue.currency?.id
        }
    },
    methods: {
        updateWallet() {
            this.$emit('updated', this.currentValue, this.walletName, this.currencyId);
            this.walletName = '';
            this.currencyId = 0;
        }
    }
}
</script>