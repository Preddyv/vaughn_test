import { defineStore } from 'pinia';
import { ref } from 'vue';
import { fetchUsers as apiFetchUsers, addUser as apiAddUser, deleteUser as apiDeleteUser } from '../services/api';

export const useUserStore = defineStore('user', () => {
  const users = ref([]);

  const fetchUsers = async () => {
    users.value = await apiFetchUsers();
  };

  const addUser = async (user) => {
    const newUser = await apiAddUser(user);
    users.value.push(newUser);
  };

  const deleteUser = async (id) => {
    await apiDeleteUser(id);
    users.value = users.value.filter(user => user.id !== id);
  };

  return {
    users,
    fetchUsers,
    addUser,
    deleteUser,
  };
});
