<template>
  <div class="container">
    <h1>User Management System</h1>
    
    <section>
      <h2>Users</h2>
      <div v-if="loading" class="loading">Loading users...</div>
      <div v-else-if="error" class="error">{{ error }}</div>
      <div v-else class="users-list">
        <div v-for="user in users" :key="user.id" class="user-card">
          <div class="user-info">
            <span class="user-name">{{ user.name }}</span>
            <span class="user-email">{{ user.email }}</span>
          </div>
          <button class="danger" @click="deleteUser(user.id)">Delete</button>
        </div>
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
        </div>
        <button type="submit" class="primary" :disabled="loading">Add User</button>
      </form>
    </section>

    <section>
      <h2>Hotels</h2>
      <div v-if="loadingHotels" class="loading">Loading hotels...</div>
      <div v-else-if="hotelError" class="error">{{ hotelError }}</div>
      <div v-else class="hotels-grid">
        <div v-for="hotel in hotels" :key="hotel.name" class="hotel-card">
          <div class="hotel-header">
            <span class="hotel-name">{{ hotel.name }}</span>
            <span :class="['hotel-status', hotel.isBooked ? 'status-booked' : 'status-available']">
              {{ hotel.isBooked ? 'Booked' : 'Available' }}
            </span>
          </div>
          <div class="hotel-location">
            <span>📍 Location: {{ formatCoordinates(hotel.latitude, hotel.longitude) }}</span>
          </div>
          <button 
            v-if="!hotel.isBooked"
            class="primary"
            @click="bookHotel(hotel)"
            :disabled="loading"
          >
            Book Now
          </button>
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

@media (max-width: 640px) {
  .form-group {
    flex-direction: column;
  }
  
  .hotels-grid {
    grid-template-columns: 1fr;
  }
  
  .user-info {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.25rem;
  }
}
</style>
