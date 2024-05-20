<template>
    <button @click="showModal = true">Add new</button>
    <CategoryList :categories="categories" @removeCategory="onRemoveCategory" @updateCategory="onUpdateCategory">
    </CategoryList>
    <CreateCategoryModal :show="showModal" @created="onCreateCategory" />
    <UpdateCategoryModal :show="showUpdateModal" @updated="updateCategory" :currentValue="categoryToUpdate" />
</template>

<script lang="ts">
import { Client } from '@/data_access/client';
import type { Category } from '../../data_access/models/category'
import CategoryList from '../lists/CategoryList.vue';
import CreateCategoryModal from '../modals/CreateCategoryModal.vue';
import UpdateCategoryModal from '../modals/UpdateCategoryModal.vue';
import type { UserContext } from '@/data_access/userContext';

export default {
    data() {
        const client = new Client();
        return {
            client,
            categories: [] as Array<Category>,
            showModal: false,
            showUpdateModal: false,
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
        CreateCategoryModal,
        UpdateCategoryModal,
    },
    methods: {
        async onCreateCategory(categoryName: string) {
            this.showModal = false;
            let result = await this.client.createCategory({ name: categoryName, userId: this.userContext.userId })
            this.categories.push(result);
        },
        async onRemoveCategory(id: number) {
            this.categories = this.categories.filter(x => x.id != id);
            await this.client.removeCategory(id);
        },
        async onUpdateCategory(id: number) {
            let category = this.categories.find(x => x.id == id);
            this.categoryToUpdate = category;
            this.showUpdateModal = true;
        },
        async updateCategory(category: Category, newName: string) {
            this.showUpdateModal = false;
            let cat = this.categories.find(x => x.id == category.id);

            if (cat == null || cat == undefined) {
                console.log("Category cannot be null");
            }
            cat!.name = newName;
            await this.client.updateCategory({ name: newName, userId: category.userId, id: category.id });
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