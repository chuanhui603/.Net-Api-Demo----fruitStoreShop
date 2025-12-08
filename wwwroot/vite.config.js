import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
   server: {
    proxy: {
      '/api': 'https://localhost:7112'
    }
  },
  plugins: [vue()]
})