import { defineStore } from 'pinia';
import { ref } from 'vue';
import { fetchUsers, addUser, deleteUser } from '../services/api';

export const useUserStore = defineStore('user', () => {
  const users = ref([]);

  const fetchUsers = async () => {
    users.value = await fetchUsers();
  };

  const addUser = async (user) => {
    const newUser = await addUser(user);
    users.value.push(newUser);
  };

  const deleteUser = async (id) => {
    await deleteUser(id);
    users.value = users.value.filter(user => user.id !== id);
  };

  return {
    users,
    fetchUsers,
    addUser,
    deleteUser,
  };
});
