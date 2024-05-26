<template>
    <dialog :open="show">
        <article>
            <header>
                <button aria-label="Close" rel="prev" @click="cancel"></button>
                Create Payee
            </header>
            <form @submit.prevent="createPayee">
                <input type="text" v-model="payeeName" required/>
                <button type="submit">Create</button>
            </form>
        </article>
    </dialog>
</template>

<script lang="ts">

export default {
    props: {
        show: Boolean
    },
    data() {
        return {
            payeeName: '',
        }
    },
    methods: {
        async createPayee() {
            this.$emit('created', this.payeeName);
            this.payeeName = '';
        },
        cancel() {
            this.$emit('cancel');
        }
    }
}
</script>