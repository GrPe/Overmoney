<template>
    <button @click="showModal = true">Add new</button>
    <CategoryList :categories="categories"></CategoryList>
    <CreateCategoryModal :show="showModal" @close="onModalClose" :context="userContext"/>
</template>

<script lang="ts">
import { Client } from '@/data_access/client';
import type { Category } from '../../data_access/models/category'
import CategoryList from '../lists/CategoryList.vue';
import CreateCategoryModal from '../modals/CreateCategoryModal.vue';
import type { UserContext } from '@/data_access/userContext';

export default {
    data() {
        return {
            categories: [] as Array<Category>,
            showModal: false,
            userContext: { userId: 1 } as UserContext
        }
    },
    mounted() {
        let client = new Client();
        client.getCategories(this.userContext.userId)
            .then((x) => { this.categories = x });
    },
    components: {
        CategoryList,
        CreateCategoryModal
    },
    methods: {
        onModalClose(category : Category) {
            this.showModal = false;
            this.categories.push(category);
        }
    }
};
</script>

<style scoped>
body #app header {
    margin: 0;
    padding: 0;
}

header {
    height: 100vh;
    width: 100vw;
    display: flex;
    align-items: center;
    justify-content: center;
}

.wrapper {
    padding-left: 30px;
}
</style>