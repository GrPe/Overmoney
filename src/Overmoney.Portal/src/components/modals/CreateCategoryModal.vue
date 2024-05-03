<template>
    <transition>
        <div v-if="show" class="modal-mask">
            <div class="modal-container">
                <div class="modal-header">
                    <p name="header">Create Category</p>
                </div>

                <div class="modal-body">
                    <form @submit.prevent="createCategory">
                        <input type="text" v-model="categoryName"/>
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
import { Client } from '@/data_access/client';
import type { UserContext } from '@/data_access/userContext'
import type { PropType } from 'vue';

export default {
    props: {
        show: Boolean,
        context: {
            type: Object as PropType<UserContext>,
            default: null
        }
    },
    data() {
        const client = new Client();

        return {
            categoryName: '',
            client
        }
    },
    methods: {
        async createCategory() {
            var result = await this.client.createCategory({name: this.categoryName, userId: this.$props.context.userId})
            this.$emit('close', result)
        }
    }
}
</script>