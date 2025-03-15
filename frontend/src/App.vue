<template>
  <div id="app">
    <h1>User List</h1>
    <UserList />
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import UserList from './components/UserList.vue';

export default defineComponent({
  name: 'App',
  components: {
    UserList,
  },
});
</script>

<style scoped>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
