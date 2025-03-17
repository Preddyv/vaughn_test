import { createRouter, createWebHistory } from 'vue-router';
import UserList from '@/components/UserList.vue';

const routes = [
  {
    path: '/',
    name: 'Home',
    component: UserList,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;