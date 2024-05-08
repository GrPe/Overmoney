<template>
    <transition>
        <div v-if="show" class="modal-mask">
            <div class="modal-container">
                <div class="modal-header">
                    <p name="header">Create Payee</p>
                </div>

                <div class="modal-body">
                    <form @submit.prevent="createPayee">
                        <input type="text" v-model="payeeName"/>
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
        }
    }
}
</script>