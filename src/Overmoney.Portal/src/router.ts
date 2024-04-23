import { createMemoryHistory, createRouter } from 'vue-router'

import MainView from './components/views/testComp.vue'
import PayeeView from './components/views/PayeeView.vue'

const routes = [
  { path: '/', component: MainView },
  { path: '/payees', component: PayeeView },
]

const router = createRouter({
  history: createMemoryHistory(),
  routes,
})

export default router;