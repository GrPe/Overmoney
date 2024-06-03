import { createRouter, createWebHistory } from "vue-router";
import axios from "axios";
import { userSessionStore } from "./data_access/sessionStore";

import MainView from "./components/views/testComp.vue";
import PayeeView from "./components/views/PayeeView.vue";
import CategoryView from "./components/views/CategoryView.vue";
import TransactionView from "./components/views/TransactionView.vue";
import SettingsView from "./components/views/SettingsView.vue";
import LoginView from "./components/views/LoginView.vue";
import RegisterView from "./components/views/RegisterView.vue";
import WalletView from "./components/views/WalletView.vue";

const routes = [
  { path: "/login", component: LoginView, meta: { requiresAuth: false } },
  { path: "/register", component: RegisterView, meta: { requiresAuth: false } },
  { path: "/", component: MainView, meta: { requiresAuth: true } },
  { path: "/wallets", component: WalletView, meta: { requiresAuth: true } },
  { path: "/payees", component: PayeeView, meta: { requiresAuth: true } },
  {
    path: "/categories",
    component: CategoryView,
    meta: { requiresAuth: true },
  },
  {
    path: "/transactions",
    component: TransactionView,
    meta: { requiresAuth: true },
  },
  { path: "/settings", component: SettingsView, meta: { requiresAuth: true } },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

const setAuthorization = (token: string) => {
  axios.defaults.headers.common.Authorization = `Bearer ${token}`;
};

router.beforeEach((to, from, next) => {
  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth);
  const session = userSessionStore();
  if (session.isAuthenticated) {
    setAuthorization(session.apiToken!);
  }
  if (requiresAuth && !session.isAuthenticated) {
    next("/login");
  } else if ((to.path === "/login" || to.path === "/register") && session.isAuthenticated) {
    next("/");
  } else {
    next();
  }
});

axios.interceptors.response.use(null, (error) => {
  if (error.response.status == 403 || error.response.status == 401) {
    const session = userSessionStore();
    session.logoutUser();
    router.push("/login");
  }

  return Promise.reject(error);
});

export default router;
