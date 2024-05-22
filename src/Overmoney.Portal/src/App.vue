<template>
  <div class="grid" style="grid-template-columns: minmax(150px, 12%) auto;">
    <header class="nav-background">
      <NavigationMenu></NavigationMenu>
    </header>
    <main class="container">
      <router-view></router-view>
    </main>
  </div>
</template>

<script lang="ts">
import NavigationMenu from './components/generic/NavigationMenu.vue';

export default {
  components: {
    NavigationMenu
  }
};
</script>