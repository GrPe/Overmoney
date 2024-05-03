<template>
    <button @click="showModal = true">Add new</button>
    <CategoryList :categories="categories" @removeCategory="onRemoveCategory"></CategoryList>
    <CreateCategoryModal :show="showModal" @created="onCreateCategory" :context="userContext" :currentValue="categoryToUpdate"/>
</template>

<script lang="ts">
import { Client } from '@/data_access/client';
import type { Category } from '../../data_access/models/category'
import CategoryList from '../lists/CategoryList.vue';
import CreateCategoryModal from '../modals/CreateCategoryModal.vue';
import type { UserContext } from '@/data_access/userContext';

export default {
    data() {
        const client = new Client();
        return {
            client,
            categories: [] as Array<Category>,
            showModal: false,
            categoryToUpdate: {} as Category | undefined,
            userContext: { userId: 1 } as UserContext
        }
    },
    mounted() {
        this.client.getCategories(this.userContext.userId)
            .then((x) => { this.categories = x });
    },
    components: {
        CategoryList,
        CreateCategoryModal
    },
    methods: {
        async onCreateCategory(categoryName : string) {
            this.showModal = false;
            let result = await this.client.createCategory({name: categoryName, userId: this.userContext.userId})
            this.categories.push(result);
        },
        async onRemoveCategory(id: number) {
            this.categories = this.categories.filter(x=> x.id != id);
            await this.client.removeCategory(id);
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