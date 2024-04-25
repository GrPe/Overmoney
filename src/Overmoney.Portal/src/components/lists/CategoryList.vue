<template>
    <table>
        <tr>
            <th>Id</th>
            <th>Name</th>
        </tr>
        <tr v-for="category in categories" :key="category.id">
            <td>{{ category.id }}</td>
            <td>{{ category.name }}</td>
        </tr>
    </table>
</template>

<script lang="ts">
import type { Category }  from '../../data_access/models/category';

export default {
    props: {
        categories: Array<Category>
    },
    created() {
        console.log(this.categories)
    }
}

</script>

<style scoped>
table, th, td {
    border: 1px solid pink
}
</style>