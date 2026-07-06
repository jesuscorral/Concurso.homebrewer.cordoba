import { expect, test } from '@playwright/test';

const pages = [
  { path: '/', title: /Concurso Homebrewer/i },
  { path: '/rules', title: /Bases/ },
  { path: '/sponsors', title: /Patrocinadores/ },
  { path: '/registration', title: /Inscripción/ },
  { path: '/organization', title: /Organización/ },
  { path: '/contact', title: /Contacto/ },
];

for (const { path, title } of pages) {
  test(`la página ${path} carga sin errores de JavaScript`, async ({ page }) => {
    const errors: string[] = [];
    page.on('pageerror', (error) => errors.push(error.message));

    const response = await page.goto(path);

    expect(response?.status()).toBeLessThan(400);
    await expect(page).toHaveTitle(title);
    await expect(page.locator('app-root')).not.toBeEmpty();
    expect(errors).toEqual([]);
  });
}

test('el menú de navegación funciona tras la hidratación', async ({ page }) => {
  await page.goto('/');
  await page.locator('nav').getByRole('link', { name: 'BASES', exact: true }).click();
  await expect(page).toHaveURL(/\/rules$/);
  await expect(page).toHaveTitle(/Bases/);
});

test('el HTML prerenderizado incluye los metadatos SEO', async ({ request }) => {
  // Se comprueba la respuesta cruda del servidor: es lo que ven los buscadores
  // antes de que arranque JavaScript.
  const response = await request.get('/rules');
  expect(response.status()).toBe(200);
  const html = await response.text();
  expect(html).toContain('<link rel="canonical" href="https://www.concursohomebrewercordoba.es/rules">');
  expect(html).toMatch(/<meta name="description" content="[^"]+"/);
  expect(html).toContain('application/ld+json');
});
