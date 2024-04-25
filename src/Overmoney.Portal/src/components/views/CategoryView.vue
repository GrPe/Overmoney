<template>
    <CategoryList :categories="categoriesList" ></CategoryList>
</template>
  
<script lang="ts">
import type { Category } from '../../data_access/models/category'
import type { Wallet} from '../../data_access/models/wallet'
import CategoryList from '../lists/CategoryList.vue';

export default {
    data() {
        let wallets: Array<Wallet> = [
            { name: "Test", id: 1, userId: 1, currency: { id: 1, code: "PLN", name: "PLN" } },
            { name: "Wallet 2", id: 2, userId: 1, currency: { id: 1, code: "PLN", name: "PLN" } }
            ];
        let categoriesList: Array<Category> = [
            { name: "Test Category", id: 1, userId: 1},
            { name: "Food", id: 2, userId: 1},
            { name: "Home", id: 3, userId: 1},
            { name: "Horse", id: 4, userId: 1},
        ];

        return {
            wallets,
            categoriesList,
        };
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