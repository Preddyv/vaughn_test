<template>
  <div class="container">
    <h1>User Management System</h1>
    
    <section>
      <h2>Users</h2>
      <div v-if="loading" class="loading">Loading users...</div>
      <div v-else-if="error" class="error">{{ error }}</div>
      <div v-else>
        <div class="table-container">
          <table class="data-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Email</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="user in users" :key="user.id">
                <td>{{ user.name }}</td>
                <td>{{ user.email }}</td>
                <td>
                  <button class="danger" @click="deleteUser(user.id)">Delete</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <form @submit.prevent="addUser" class="form">
          <div class="form-group">
            <input 
              v-model="newUser.name" 
              placeholder="Enter name" 
              required
              aria-label="User name"
            />
            <input 
              v-model="newUser.email" 
              type="email" 
              placeholder="Enter email" 
              required
              aria-label="User email"
            />
            <button type="submit" class="primary" :disabled="loading">Add User</button>
          </div>
        </form>
      </div>
    </section>

    <section>
      <h2>Hotels</h2>
      <div v-if="loadingHotels" class="loading">Loading hotels...</div>
      <div v-else-if="hotelError" class="error">{{ hotelError }}</div>
      <div v-else>
        <div class="table-container">
          <table class="data-table">
            <thead>
              <tr>
                <th>Hotel Name</th>
                <th>Location</th>
                <th>Nearest User</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="hotel in hotels" :key="hotel.name">
                <td>{{ hotel.name }}</td>
                <td>{{ formatCoordinates(hotel.latitude, hotel.longitude) }}</td>
                <td>{{ hotel.nearestUser || 'N/A' }}</td>

                <td>
                  <span :class="['status-badge', hotel.isBooked ? 'status-booked' : 'status-available']">
                    {{ hotel.isBooked ? 'Booked' : 'Available' }}
                  </span>
                </td>
                <td>
                  <button 
                    v-if="!hotel.isBooked"
                    class="primary"
                    @click="bookHotel(hotel)"
                    :disabled="loading"
                  >
                    Book Now
                  </button>
                  <span v-else class="booked-text">Booked</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </section>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useUserStore } from '../store';
import { bookHotel as apiBookHotel, fetchHotels as apiFetchHotels } from '../services/api';
import type { Hotel } from '../services/api';

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
    const hotels = ref<Hotel[]>([]);
    const loading = ref<boolean>(false);
    const loadingHotels = ref<boolean>(false);
    const error = ref<string | null>(null);
    const hotelError = ref<string | null>(null);

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

    const fetchHotels = async () => {
      loadingHotels.value = true;
      hotelError.value = null;
      
      try {
        hotels.value = await apiFetchHotels();
      } catch (err) {
        hotelError.value = err instanceof Error ? err.message : 'An error occurred while fetching hotels';
      } finally {
        loadingHotels.value = false;
      }
    };

    const addUser = async () => {
      loading.value = true;
      error.value = null;
      
      try {
        await userStore.addUser(newUser.value);
        newUser.value = { name: '', email: '' };
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
        users.value = userStore.users;
      } catch (err) {
        error.value = err instanceof Error ? err.message : 'An error occurred while deleting a user';
      } finally {
        loading.value = false;
      }
    };

    const bookHotel = async (hotel: Hotel) => {
      loading.value = true;
      error.value = null;

      try {
        await apiBookHotel(hotel);
        // Refresh hotel list after booking
        await fetchHotels();
      } catch (err) {
        error.value = err instanceof Error ? err.message : 'An error occurred while booking the hotel';
      } finally {
        loading.value = false;
      }
    };

    const formatCoordinates = (lat: number, lng: number): string => {
      return `${lat.toFixed(4)}°, ${lng.toFixed(4)}°`;
    };

    onMounted(() => {
      fetchUsers();
      fetchHotels();
    });

    return {
      users,
      hotels,
      newUser,
      loading,
      loadingHotels,
      error,
      hotelError,
      addUser,
      deleteUser,
      bookHotel,
      formatCoordinates,
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

.hotels-list {
  margin-top: 20px;
  display: grid;
  gap: 20px;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
}

.hotel-item {
  border: 1px solid #ccc;
  padding: 15px;
  border-radius: 8px;
  background-color: #f9f9f9;
}

.hotel-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
}

.hotel-location {
  color: #666;
  font-size: 0.9em;
  margin-bottom: 10px;
}

.booked {
  color: #e53935;
  font-weight: bold;
}

.available {
  color: #43a047;
  font-weight: bold;
}

button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.user-info {
  display: flex;
  gap: 1rem;
  align-items: center;
}

.user-info strong {
  font-weight: 600;
}

.user-info span {
  color: #64748b;
}

.form-group {
  display: flex;
  gap: 1rem;
  flex: 1;
}

.add-user-form {
  margin-top: 2rem;
}

.table-container {
  background: white;
  border-radius: 0.5rem;
  box-shadow: var(--shadow);
  overflow: hidden;
  margin: 1.5rem 0;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
  text-align: left;
}

.data-table th,
.data-table td {
  padding: 1rem;
  border-bottom: 1px solid var(--gray-200);
}

.data-table th {
  background: var(--gray-50);
  font-weight: 600;
  color: var(--gray-700);
  white-space: nowrap;
}

.data-table tr:last-child td {
  border-bottom: none;
}

.data-table tbody tr:hover {
  background-color: var(--gray-50);
}

.status-badge {
  padding: 0.25rem 0.75rem;
  border-radius: 9999px;
  font-size: 0.75rem;
  font-weight: 500;
  display: inline-block;
}

.status-available {
  background-color: #dcfce7;
  color: var(--success-color);
}

.status-booked {
  background-color: #fee2e2;
  color: var(--danger-color);
}

.booked-text {
  color: var(--gray-600);
  font-size: 0.875rem;
  font-style: italic;
}

.form {
  background: white;
  padding: 1.5rem;
  border-radius: 0.5rem;
  box-shadow: var(--shadow);
  margin: 1.5rem 0;
}

.form-group {
  display: flex;
  gap: 1rem;
  align-items: center;
}

button {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 0.375rem;
  font-weight: 500;
  font-size: 0.875rem;
  cursor: pointer;
  transition: all 0.15s;
}

button.primary {
  background-color: var(--primary-color);
  color: white;
}

button.primary:hover {
  background-color: var(--primary-dark);
}

button.danger {
  background-color: white;
  color: var(--danger-color);
  border: 1px solid var(--gray-200);
}

button.danger:hover {
  background-color: #fee2e2;
  border-color: var(--danger-color);
}

button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

input {
  padding: 0.5rem 1rem;
  border: 1px solid var(--gray-300);
  border-radius: 0.375rem;
  font-size: 0.875rem;
  flex: 1;
}

input:focus {
  outline: none;
  border-color: var(--primary-color);
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

@media (max-width: 768px) {
  .table-container {
    overflow-x: auto;
  }
  
  .form-group {
    flex-direction: column;
    align-items: stretch;
  }
  
  button {
    width: 100%;
  }
}
</style>
