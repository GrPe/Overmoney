<template>
    <table>
        <thead>
            <tr>
                <th>Id</th>
                <th>Name</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="category in categories" :key="category.id" @click="updateCategory(category.id)">
                <td>{{ category.id }}</td>
                <td>{{ category.name }}</td>
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
        async updateCategory(id: number) {
            this.$emit('updateCategory', id);
        }
    }
}
</script>