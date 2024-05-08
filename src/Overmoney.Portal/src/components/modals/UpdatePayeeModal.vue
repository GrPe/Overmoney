<template>
    <transition>
        <div v-if="show" class="modal-mask">
            <div class="modal-container">
                <div class="modal-header">
                    <p name="header">Update Payee</p>
                </div>

                <div class="modal-body">
                    <form @submit.prevent="updatePayee">
                        <input type="text" v-model="payeeName"/>
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
        currentValue: function(newValue : Payee) {
            this.payeeName = newValue.name;
        }
    },
    methods: {
        updatePayee() {
            this.$emit('updated', this.currentValue, this.payeeName);
            this.payeeName = '';
        }
    }
}
</script>