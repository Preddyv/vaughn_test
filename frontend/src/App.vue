<template>
  <main>
    <router-view></router-view>
  </main>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'App'
});
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600&display=swap');

#app {
  min-height: 100vh;
  background-color: var(--background-color);
}

main {
  padding: 2rem 0;
}
</style>
