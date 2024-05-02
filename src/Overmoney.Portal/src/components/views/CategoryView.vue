<template>
    <CategoryList :categories="categories" ></CategoryList>
</template>
  
<script lang="ts">
import { Client } from '@/data_access/client';
import type { Category }  from '../../data_access/models/category'
import CategoryList from '../lists/CategoryList.vue';

export default {
    data() {
        return {
            categories: [] as Array<Category>
        }  
    },
    mounted() {
        let client = new Client();
        client.getCategories(1)
            .then((x) => { this.categories = x });
    },
    components: {
        CategoryList,
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