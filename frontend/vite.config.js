import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    host: "0.0.0.0",
    proxy: {
      '/api': {
        target: 'https://localhost:7117/',
        changeOrigin: true,
        secure: false,
      }
    }
  }
});
