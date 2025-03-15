import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'http://localhost:5000/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

export const fetchUsers = async () => {
  const response = await apiClient.get('/users');
  return response.data;
};

export const addUser = async (user) => {
  const response = await apiClient.post('/users', user);
  return response.data;
};

export const updateUser = async (id, user) => {
  const response = await apiClient.put(`/users/${id}`, user);
  return response.data;
};

export const deleteUser = async (id) => {
  await apiClient.delete(`/users/${id}`);
};
