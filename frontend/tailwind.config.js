/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        "graphite" : "2E2E2E",
        "graphite-light" : "3e3e3e"
      }
    },
  },
  plugins: [],
}

