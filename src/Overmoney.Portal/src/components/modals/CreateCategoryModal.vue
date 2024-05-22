<template>
    <dialog :open="show">
        <article>
            <header>
                <button aria-label="Close" rel="prev" @click="cancel"></button>
                Create Category
            </header>
            <form @submit.prevent="createCategory">
                <input type="text" v-model="categoryName" />
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
            categoryName: '',
        }
    },
    methods: {
        async createCategory() {
            this.$emit('created', this.categoryName);
            this.categoryName = '';
        },
        cancel() {
            this.$emit('cancel');
        }
    }
}
</script>