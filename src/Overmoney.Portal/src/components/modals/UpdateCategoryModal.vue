<template>
    <dialog :open="show">
        <article>
            <header>
                <button aria-label="Close" rel="prev" @click="cancel"></button>
                Update Category
            </header>
            <form @submit.prevent="updateCategory">
                <input type="text" v-model="categoryName" required/>
                <button type="submit">Update</button>
                <input type="button" class="delete" value="Delete" @click="removeCategory()"/>
            </form>
        </article>
    </dialog>
</template>
<script lang="ts">
import type { Category } from '../../data_access/models/category';
import type { PropType } from 'vue';

export default {
    props: {
        show: Boolean,
        currentValue: {
            type: Object as PropType<Category>
        }
    },
    data() {
        return {
            categoryName: this.currentValue?.name,
        }
    },
    watch: {
        currentValue: function (newValue: Category) {
            this.categoryName = newValue.name;
        }
    },
    methods: {
        updateCategory() {
            this.$emit('updated', this.currentValue, this.categoryName);
            this.categoryName = '';
        },
        cancel() {
            this.$emit('cancel');
        },
        async removeCategory() {
            this.$emit('removeCategory', this.currentValue?.id);
            this.$emit('cancel');
        }
    }
}
</script>