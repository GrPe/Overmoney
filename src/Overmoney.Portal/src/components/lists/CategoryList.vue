<template>
    <table>
        <thead>
            <tr>
                <th>Id</th>
                <th>Name</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="category in categories" :key="category.id" @click="updateCategory(category.id)">
                <td>{{ category.id }}</td>
                <td>{{ category.name }}</td>
                <td>
                    <div class="grid">
                        <button class="delete" @click="removeCategory(category.id)">
                            Delete
                        </button>
                    </div>
                </td>
            </tr>
        </tbody>
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