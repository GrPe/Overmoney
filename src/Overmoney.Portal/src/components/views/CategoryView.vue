<template>
    <nav class="top">
        <ul>
            <strong>Categories</strong>
        </ul>
        <ul>
            <button @click="showModal = true">Add new</button>
        </ul>
    </nav>
    <CategoryList :categories="categories" @updateCategory="onUpdateCategory">
    </CategoryList>
    <CreateCategoryModal :show="showModal" @created="onCreateCategory" @cancel="showModal = false" />
    <UpdateCategoryModal :show="showUpdateModal" @updated="updateCategory" :currentValue="categoryToUpdate"
        @cancel="showUpdateModal = false" @removeCategory="onRemoveCategory" :disableRemove="disableRemove"/>
</template>

<script lang="ts">
import { Client } from '@/data_access/client';
import type { Category } from '../../data_access/models/category'
import CategoryList from '../lists/CategoryList.vue';
import CreateCategoryModal from '../modals/CreateCategoryModal.vue';
import UpdateCategoryModal from '../modals/UpdateCategoryModal.vue';
import { userSessionStore } from '@/data_access/sessionStore';

export default {
    data() {
        const client = new Client();
        const session = userSessionStore();
        return {
            client,
            categories: [] as Array<Category>,
            showModal: false,
            showUpdateModal: false,
            disableRemove: false,
            categoryToUpdate: {} as Category | undefined,
            session
        }
    },
    mounted() {
        this.client.getCategories(this.session.userId)
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
            let result = await this.client.createCategory({ name: categoryName, userId: this.session.userId })
            this.categories.push(result);
        },
        async onRemoveCategory(id: number) {
            this.categories = this.categories.filter(x => x.id != id);
            await this.client.removeCategory(id);
        },
        async onUpdateCategory(id: number) {
            let transactions = await this.client.getTransactionsByCategory(this.session.userId, id);

            if(transactions != null && transactions.length > 0) {
                this.disableRemove = true;
            } else {
                this.disableRemove = false;
            }

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