import { createApp } from 'vue'
import { createPinia } from 'pinia';
import piniaPluginPersistedstate  from 'pinia-plugin-persistedstate';
import App from './App.vue'
import router from './router';

import "./assets/main.css"
import "./assets/pico.jade.min.css"

const pinia = createPinia();
pinia.use(piniaPluginPersistedstate);

const app = createApp(App);
app.use(router);
app.use(pinia);

app.mount('#app')
