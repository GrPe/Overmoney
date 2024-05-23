<template>
    <header v-if="userSession.isAuthenticated" class="nav-background">
      <NavigationMenu></NavigationMenu>
    </header>
    <div class="grid" :style="userSession.isAuthenticated ? authStyle : ''">
      <div v-if="userSession.isAuthenticated">
        <!-- empty -->
      </div>
      <main class="container">
        <router-view></router-view>
      </main>
    </div>
</template>

<script lang="ts">
import NavigationMenu from './components/generic/NavigationMenu.vue';
import { userSessionStore } from './data_access/sessionStore';

export default {
  data() {
    const userSession = userSessionStore();
    const authStyle = "grid-template-columns: minmax(150px, 12%) auto;"

    return {
      userSession,
      authStyle
    }
  },
  components: {
    NavigationMenu
  }
};
</script>