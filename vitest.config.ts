import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    environment: 'jsdom', // Esto le dice a Vitest que simule el objeto `document`
    coverage: {
      provider: 'v8',
      reporter: ['text', 'lcov'],
    },
  },
});