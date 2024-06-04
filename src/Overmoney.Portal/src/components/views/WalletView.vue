<template>
    <nav class="top">
        <ul>
            <strong>Wallets</strong>
        </ul>
        <ul>
            <button @click="showModal = true">Add new</button>
        </ul>
    </nav>
    <WalletList :wallets="wallets" @updateWallet="onUpdateWallet">
    </WalletList>
    <CreateWalletModal :show="showModal" :currencies="currencies" @created="onCreateWallet"
        @cancel="showModal = false" />
    <UpdateWalletModal :show="showUpdateModal" :currencies="currencies" @updated="updateWallet"
        :currentValue="walletToUpdate" @cancel="showUpdateModal = false" @removeWallet="onRemoveWallet" :disableRemove="disableRemove" />
</template>

<script lang="ts">
import { Client } from '@/data_access/client';
import CreateWalletModal from '../modals/CreateWalletModal.vue';
import UpdateWalletModal from '../modals/UpdateWalletModal.vue';
import { userSessionStore } from '@/data_access/sessionStore';
import WalletList from '../lists/WalletList.vue';
import type { Wallet } from '@/data_access/models/wallet';
import type { Currency } from '@/data_access/models/currency';

export default {
    data() {
        const client = new Client();
        const session = userSessionStore();
        return {
            client,
            wallets: [] as Array<Wallet>,
            currencies: [] as Array<Currency>,
            showModal: false,
            showUpdateModal: false,
            disableRemove: false,
            walletToUpdate: {} as Wallet | undefined,
            session
        }
    },
    mounted() {
        this.client.getWallets(this.session.userId)
            .then((x) => { this.wallets = x });
        this.client.getCurrencies()
            .then((x) => { this.currencies = x });
    },
    components: {
        WalletList,
        CreateWalletModal,
        UpdateWalletModal,
    },
    methods: {
        async onCreateWallet(walletName: string, currencyId: number) {
            this.showModal = false;
            let result = await this.client.createWallet({ name: walletName, userId: this.session.userId, currencyId })
            this.wallets.push(result);
        },
        async onRemoveWallet(id: number) {
            this.wallets = this.wallets.filter(x => x.id != id);
            await this.client.removeWallet(id);
        },
        async onUpdateWallet(id: number) {
            let transactions = await this.client.getTransactionsByWallet(this.session.userId, id);

            console.log(transactions);

            if (transactions != null && transactions != undefined && transactions.length > 0) {
                this.disableRemove = true;
            } else {
                this.disableRemove = false;
            }

            let wallet = this.wallets.find(x => x.id == id);
            this.walletToUpdate = wallet;
            this.showUpdateModal = true;
        },
        async updateWallet(wallet: Wallet, newName: string, newCurrency: number) {
            this.showUpdateModal = false;
            let wall = this.wallets.find(x => x.id == wallet.id);

            if (wall == null || wall == undefined) {
                console.log("Wallet cannot be null");
            }
            wall!.name = newName;
            wall!.currency = this.currencies.find(x => x.id == newCurrency)!;
            await this.client.updateWallet({ name: newName, userId: this.session.userId, id: wallet.id, currencyId: newCurrency });
        }
    }
};
</script>