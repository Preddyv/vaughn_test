<template>
  <div>
    <h2>Users</h2>
    <div v-if="loading" class="loading">Loading users...</div>
    <div v-else-if="error" class="error">Error: {{ error }}</div>
    <div v-else>
      <ul>
        <li v-for="user in users" :key="user.id">
          {{ user.name }} - {{ user.email }}
          <button @click="deleteUser(user.id)">Delete</button>
        </li>
      </ul>
      <div v-if="users.length === 0" class="no-users">No users found.</div>
    </div>
    <form @submit.prevent="addUser">
      <input v-model="newUser.name" placeholder="Name" required />
      <input v-model="newUser.email" placeholder="Email" required />
      <button type="submit" :disabled="loading">Add User</button>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useUserStore } from '../store';

interface User {
  id: number;
  name: string;
  email: string;
}

interface NewUser {
  name: string;
  email: string;
}

export default defineComponent({
  name: 'UserList',
  setup() {
    const userStore = useUserStore();
    const users = ref<User[]>([]);
    const newUser = ref<NewUser>({ name: '', email: '' });
    const loading = ref<boolean>(false);
    const error = ref<string | null>(null);

    const fetchUsers = async () => {
      loading.value = true;
      error.value = null;
      
      try {
        await userStore.fetchUsers();
        users.value = userStore.users;
      } catch (err) {
        error.value = err instanceof Error ? err.message : 'An error occurred while fetching users';
      } finally {
        loading.value = false;
      }
    };

    const addUser = async () => {
      loading.value = true;
      error.value = null;
      
      try {
        await userStore.addUser(newUser.value);
        newUser.value = { name: '', email: '' };
        // Refresh the user list from store
        users.value = userStore.users;
      } catch (err) {
        error.value = err instanceof Error ? err.message : 'An error occurred while adding a user';
      } finally {
        loading.value = false;
      }
    };

    const deleteUser = async (id: number) => {
      loading.value = true;
      error.value = null;
      
      try {
        await userStore.deleteUser(id);
        // Refresh the user list from store
        users.value = userStore.users;
      } catch (err) {
        error.value = err instanceof Error ? err.message : 'An error occurred while deleting a user';
      } finally {
        loading.value = false;
      }
    };

    onMounted(fetchUsers);

    return {
      users,
      newUser,
      loading,
      error,
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

.loading {
  color: #666;
  font-style: italic;
  margin: 20px 0;
}

.error {
  color: #f44336;
  margin: 20px 0;
}

.no-users {
  color: #666;
  font-style: italic;
  margin: 20px 0;
}

form {
  margin-top: 20px;
  display: flex;
  gap: 10px;
}

input {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}
</style>
