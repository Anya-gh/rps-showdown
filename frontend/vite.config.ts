/// <reference types="vitest" />
/// <reference types="vite/client" />

import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  base: process.env.NODE_ENV === 'production' ? '/<repo-name>' : '/',
  test: { 
    environment: "jsdom",
    globals: true,
    setupFiles: './src/tests/setup.ts'
  }
})
