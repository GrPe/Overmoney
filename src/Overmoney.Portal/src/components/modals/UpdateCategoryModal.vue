<template>
    <transition>
        <div v-if="show" class="modal-mask">
            <div class="modal-container">
                <div class="modal-header">
                    <p name="header">Update Category</p>
                </div>

                <div class="modal-body">
                    <form @submit.prevent="updateCategory">
                        <input type="text" v-model="categoryName"/>
                        <button type="submit">Update</button>
                    </form>
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </transition>
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
        currentValue: function(newValue : Category) {
            this.categoryName = newValue.name;
        }
    },
    methods: {
        updateCategory() {
            this.$emit('updated', this.currentValue, this.categoryName);
            this.categoryName = '';
        }
    }
}
</script>