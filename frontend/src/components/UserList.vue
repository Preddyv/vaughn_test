<template>
  <div>
    <h2>Users</h2>
    <ul>
      <li v-for="user in users" :key="user.id">
        {{ user.name }} - {{ user.email }}
        <button @click="deleteUser(user.id)">Delete</button>
      </li>
    </ul>
    <form @submit.prevent="addUser">
      <input v-model="newUser.name" placeholder="Name" required />
      <input v-model="newUser.email" placeholder="Email" required />
      <button type="submit">Add User</button>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useUserStore } from '../store';

export default defineComponent({
  name: 'UserList',
  setup() {
    const userStore = useUserStore();
    const users = ref(userStore.users);
    const newUser = ref({ name: '', email: '' });

    const fetchUsers = async () => {
      await userStore.fetchUsers();
    };

    const addUser = async () => {
      await userStore.addUser(newUser.value);
      newUser.value = { name: '', email: '' };
    };

    const deleteUser = async (id: number) => {
      await userStore.deleteUser(id);
    };

    onMounted(fetchUsers);

    return {
      users,
      newUser,
      addUser,
      deleteUser,
    };
  },
});
</script>

<style scoped>
ul {
  list-style-type: none;
  padding: 0;
}

li {
  margin: 10px 0;
}

button {
  margin-left: 10px;
}
</style>
