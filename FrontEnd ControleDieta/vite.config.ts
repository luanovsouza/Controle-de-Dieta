import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      // Auth endpoints: /user/login, /user/Register
      '/user': {
        target: 'http://localhost:5094',
        changeOrigin: true,
        secure: false,
      },
      // All /api/* routes (UserMetaCal, ProcessFoodIa, Refeicoes, etc.)
      '/api': {
        target: 'http://localhost:5094',
        changeOrigin: true,
        secure: false,
      },
      // AI Recipe route
      '/ReceitasIa': {
        target: 'http://localhost:5094',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})
