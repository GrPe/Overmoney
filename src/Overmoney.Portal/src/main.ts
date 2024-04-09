//import './assets/main.css'
import 'primevue/resources/themes/aura-light-green/theme.css'
import 'primevue/resources/primevue.min.css';
//import 'primeicons/primeicons.css';     

import { createApp } from 'vue'
import App from './App.vue'

import PrimeVue from 'primevue/config'
import PrimeMenu from "primevue/menubar";
import Panel from "primevue/panel"

const app = createApp(App);
app.use(PrimeVue)

app.component("PrimeMenu", PrimeMenu);
app.component("PrimePanel", Panel);

app.mount('#app')
