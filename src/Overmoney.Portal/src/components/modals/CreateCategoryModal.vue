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

export default {
    props: {
        show: Boolean
    },
    data() {
        return {
            categoryName: '',
        }
    },
    methods: {
        async createCategory() {
            this.$emit('created', this.categoryName);
            this.categoryName = '';
        }
    }
}
</script>