import { createMemoryHistory, createRouter } from 'vue-router'

import MainView from './components/views/testComp.vue'
import PayeeView from './components/views/PayeeView.vue'
import CategoryView from './components/views/CategoryView.vue'
import TransactionView from './components/views/TransactionView.vue'
import SettingsView from './components/views/SettingsView.vue'

const routes = [
  { path: '/', component: MainView },
  { path: '/payees', component: PayeeView },
  { path: '/categories', component: CategoryView },
  { path: '/transactions', component: TransactionView },
  { path: '/settings', component: SettingsView }
]

const router = createRouter({
  history: createMemoryHistory(),
  routes,
})

export default router;