<template>
  <Navigation :wallets="wallets"></Navigation>
  <main>
  </main>
</template>

<script lang="ts">
import Navigation from './components/NavigationMenu.vue'
import type { Wallet } from './data_access/models/wallet'

export default {
  data() {
    let wallets: Array<Wallet> = [
      { name: "Test", id: 1, userId: 1, currency: { id: 1, code: "PLN", name: "PLN" } },
      { name: "Wallet 2", id: 2, userId: 1, currency: { id: 1, code: "PLN", name: "PLN" } }
    ]

    return {
      wallets
    };
  },
  components: {
    Navigation
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