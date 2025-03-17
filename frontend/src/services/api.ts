import axios, { AxiosError } from 'axios';

// Define interfaces for API data structures
export interface User {
  id: number;
  name: string;
  email: string;
  username?: string;
  phone?: string;
  website?: string;
  address?: Address;
  company?: Company;
}

export interface Address {
  street?: string;
  suite?: string;
  city?: string;
  zipcode?: string;
  geo?: Geo;
}

export interface Geo {
  lat: number;
  lng: number;
}

export interface Company {
  name?: string;
  catchPhrase?: string;
  bs?: string;
}

export interface NewUser {
  name: string;
  email: string;
}

// API client setup
const apiClient = axios.create({
  baseURL: 'http://localhost:5073/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Handle API errors consistently
const handleApiError = (error: unknown): never => {
  if (axios.isAxiosError(error)) {
    const axiosError = error as AxiosError;
    if (axiosError.response) {
      throw new Error(`API Error: ${axiosError.response.status} - ${axiosError.response.statusText}`);
    } else if (axiosError.request) {
      throw new Error('No response received from the server. Please check your connection.');
    }
  }
  throw new Error('An unexpected error occurred');
};

// API methods
export const fetchUsers = async (): Promise<User[]> => {
  try {
    const response = await apiClient.get<User[]>('/users');
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

export const addUser = async (user: NewUser): Promise<User> => {
  try {
    const response = await apiClient.post<User>('/users', user);
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

export const updateUser = async (id: number, user: User): Promise<User> => {
  try {
    const response = await apiClient.put<User>(`/users/${id}`, user);
    return response.data;
  } catch (error) {
    return handleApiError(error);
  }
};

export const deleteUser = async (id: number): Promise<void> => {
  try {
    const response = await apiClient.delete(`/users/${id}`);
    if (response.status !== 204) {
      throw new Error(`Unexpected status code: ${response.status}`);
    }
  } catch (error) {
    if (axios.isAxiosError(error)) {
      const axiosError = error as AxiosError;
      if (axiosError.response) {
        throw new Error(`Failed to delete user: ${axiosError.response.status} - ${axiosError.response.statusText}`);
      } else if (axiosError.request) {
        throw new Error('No response received from the server. Please check your connection.');
      }
    }
    throw new Error('An unexpected error occurred while deleting the user');
  }
};
