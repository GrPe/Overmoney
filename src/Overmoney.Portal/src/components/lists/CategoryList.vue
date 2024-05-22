<template>
    <table class="striped">
        <thead>
            <th>Id</th>
            <th>Name</th>
            <th>Actions</th>
        </thead>
        <tr v-for="category in categories" :key="category.id">
            <td>{{ category.id }}</td>
            <td>{{ category.name }}</td>
            <td>
                <div class="grid">
                    <button @click="updateCategory(category.id)">
                        Edit
                    </button>
                    <button class="delete" @click="removeCategory(category.id)">
                        Delete
                    </button>
                </div>
            </td>
        </tr>
    </table>
</template>

<script lang="ts">
import type { Category } from '../../data_access/models/category';

export default {
    props: {
        categories: Array<Category>
    },
    methods: {
        async removeCategory(id: number) {
            this.$emit('removeCategory', id);
        },
        async updateCategory(id: number) {
            this.$emit('updateCategory', id);
        }
    }
}
</script>