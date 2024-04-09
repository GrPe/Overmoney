<script lang="ts">
export default {
    data() {
        return {
            items: [
                {
                    label: 'New',
                    icon: 'pi pi-plus',
                    // command: () => {
                    //     this.$toast.add({ severity: 'success', summary: 'Success', detail: 'File created', life: 3000 });
                    // }
                },
                {
                    label: 'Search',
                    icon: 'pi pi-search',
                    // command: () => {
                    //     this.$toast.add({ severity: 'warn', summary: 'Search Completed', detail: 'No results found', life: 3000 });
                    // }
                }
            ]
        };
    }
};
</script>

<template>
    <div class="card flex justify-content-center">
      <PrimeMenu :model="items"></PrimeMenu>
    </div>
    <main>
      test test test
      <p>test test</p>
      tes test test
    </main>
</template>

<style scoped>
body #app header {
  margin: 0;
  padding: 0;
}
header{
  height: 100vh;
  width: 100vw;
  display:flex;
  align-items:center;
  justify-content:center;
}
.wrapper{
  padding-left: 30px;
}
</style>