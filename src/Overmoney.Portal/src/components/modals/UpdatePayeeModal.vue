<template>
    <dialog :open="show">
        <article>
            <header>
                <button aria-label="Close" rel="prev" @click="cancel"></button>
                Update Payee
            </header>
            <form @submit.prevent="updatePayee">
                <input type="text" v-model="payeeName" />
                <button type="submit">Update</button>
            </form>
        </article>
    </dialog>
</template>

<script lang="ts">
import type { Payee } from '../../data_access/models/payee';
import type { PropType } from 'vue';

export default {
    props: {
        show: Boolean,
        currentValue: {
            type: Object as PropType<Payee>
        }
    },
    data() {
        return {
            payeeName: this.currentValue?.name,
        }
    },
    watch: {
        currentValue: function (newValue: Payee) {
            this.payeeName = newValue.name;
        }
    },
    methods: {
        updatePayee() {
            this.$emit('updated', this.currentValue, this.payeeName);
            this.payeeName = '';
        },
        cancel() {
            this.$emit('cancel');
        }
    }
}
</script>