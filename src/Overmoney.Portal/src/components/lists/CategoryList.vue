<template>
    <table>
        <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Actions</th>
        </tr>
        <tr v-for="category in categories" :key="category.id">
            <td>{{ category.id }}</td>
            <td>{{ category.name }}</td>
            <td>
                <button @click="updateCategory(category.id)">
                    Edit
                </button>
                <button @click="removeCategory(category.id)">
                    Delete
                </button>
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

<style scoped>
table,
th,
td {
    border: 1px solid pink
}
</style>