<template>
    <nav class="top">
        <ul>
            <strong>Payees</strong>
        </ul>
        <ul>
            <button @click="showModal = true">Add new</button>
        </ul>
    </nav>
    <PayeesList :payees="payees" @updatePayee="onUpdatePayee">
    </PayeesList>
    <CreatePayeeModal :show="showModal" @created="onCreatePayee" @cancel="showModal = false" />
    <UpdatePayeeModal :show="showUpdateModal" @updated="updatePayee" :currentValue="payeeToUpdate"
        @cancel="showUpdateModal = false" @removePayee="onRemovePayee" :disableRemove="disableRemove" />
</template>

<script lang="ts">
import { Client } from '@/data_access/client';
import type { Payee } from '../../data_access/models/payee'
import CreatePayeeModal from '../modals/CreatePayeeModal.vue';
import UpdatePayeeModal from '../modals/UpdatePayeeModal.vue';
import PayeesList from '../lists/PayeesList.vue';
import { userSessionStore } from '@/data_access/sessionStore';

export default {
    data() {
        const client = new Client();
        const session = userSessionStore();
        return {
            client,
            payees: [] as Array<Payee>,
            showModal: false,
            showUpdateModal: false,
            disableRemove: false,
            payeeToUpdate: {} as Payee | undefined,
            session
        }
    },
    mounted() {
        this.client.getPayees(this.session.userId)
            .then((x) => { this.payees = x });
    },
    components: {
        PayeesList,
        CreatePayeeModal,
        UpdatePayeeModal,
    },
    methods: {
        async onCreatePayee(payeeName: string) {
            this.showModal = false;
            let result = await this.client.createPayee({ name: payeeName, userId: this.session.userId })
            this.payees.push(result);
        },
        async onRemovePayee(id: number) {
            this.payees = this.payees.filter(x => x.id != id);
            await this.client.removePayee(id);
        },
        async onUpdatePayee(id: number) {
            let transactions = await this.client.getTransactionsByPayee(this.session.userId, id);

            if (transactions != null && transactions.length > 0) {
                this.disableRemove = true;
            } else {
                this.disableRemove = false;
            }

            let payee = this.payees.find(x => x.id == id);
            this.payeeToUpdate = payee;
            this.showUpdateModal = true;
        },
        async updatePayee(payee: Payee, newName: string) {
            this.showUpdateModal = false;
            let pay = this.payees.find(x => x.id == payee.id);

            if (pay == null || pay == undefined) {
                console.log("Payee cannot be null");
            }
            pay!.name = newName;
            await this.client.updatePayee({ name: newName, userId: payee.userId, id: payee.id });
        }
    }
};
</script>