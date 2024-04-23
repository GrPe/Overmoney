<template>
  <NavigationMenu></NavigationMenu>
  <main>
    <router-view></router-view>
  </main>
</template>

<script lang="ts">
import NavigationMenu from './components/generic/NavigationMenu.vue';

export default {
  components:{
    NavigationMenu
  }
};
</script>

<style scoped>
</style>