import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'

const variableNotDefinedMessage =
  'Environment variable VITE_API_URI not defined'
if (import.meta.env.VITE_API_URI === undefined) {
  alert(variableNotDefinedMessage)
  throw Error(variableNotDefinedMessage)
}

createApp(App).mount('#app')
