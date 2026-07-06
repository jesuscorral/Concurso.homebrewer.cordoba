import { defineConfig, devices } from '@playwright/test';

const PORT = 4173;

/**
 * Smoke tests e2e sobre el sitio prerenderizado (contenido de dist/).
 * Requiere un build previo: `npm run build && npm run e2e`.
 */
export default defineConfig({
  testDir: './e2e',
  fullyParallel: true,
  forbidOnly: !!process.env['CI'],
  retries: process.env['CI'] ? 2 : 0,
  reporter: process.env['CI']
    ? [['list'], ['html', { open: 'never' }]]
    : [['list']],
  use: {
    baseURL: `http://127.0.0.1:${PORT}`,
    trace: 'on-first-retry',
  },
  webServer: {
    command: `http-server dist -a 127.0.0.1 -p ${PORT} -c-1 --silent`,
    url: `http://127.0.0.1:${PORT}/`,
    reuseExistingServer: !process.env['CI'],
  },
  projects: [
    { name: 'chromium', use: { ...devices['Desktop Chrome'] } },
    { name: 'firefox', use: { ...devices['Desktop Firefox'] } },
    { name: 'webkit', use: { ...devices['Desktop Safari'] } },
  ],
});
