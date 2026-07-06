import { TestBed } from '@angular/core/testing';
import { Router, provideRouter } from '@angular/router';
import { beforeEach, describe, expect, it } from 'vitest';

import { routes } from '../../app.routes';
import { GlobalConstants } from '../global-constants';
import { SeoService } from './seo.service';

const SITE_URL = 'https://www.concursohomebrewercordoba.es';

function metaContent(selector: string): string | null {
  return document.head.querySelector<HTMLMetaElement>(selector)?.content ?? null;
}

function canonicalHref(): string | null {
  return document.head
    .querySelector<HTMLLinkElement>('link[rel="canonical"]')
    ?.getAttribute('href') ?? null;
}

describe('SeoService', () => {
  let router: Router;

  beforeEach(() => {
    // El <head> de jsdom se comparte entre tests: limpia lo que deja el servicio.
    document.getElementById('ld-organization')?.remove();
    document.getElementById('ld-event')?.remove();
    document.head.querySelector('link[rel="canonical"]')?.remove();

    TestBed.configureTestingModule({ providers: [provideRouter(routes)] });
    router = TestBed.inject(Router);
    TestBed.inject(SeoService).init();
  });

  it('añade el JSON-LD de la organización al iniciarse', () => {
    const script = document.getElementById('ld-organization');
    expect(script).not.toBeNull();
    const data = JSON.parse(script!.textContent!);
    expect(data['@type']).toBe('Organization');
    expect(data.url).toBe(SITE_URL);
  });

  it('en la portada publica canónica, metas y el JSON-LD del evento', async () => {
    await router.navigateByUrl('/');

    expect(canonicalHref()).toBe(`${SITE_URL}/`);
    expect(metaContent('meta[property="og:url"]')).toBe(`${SITE_URL}/`);
    expect(metaContent('meta[name="description"]')).toBeTruthy();
    expect(metaContent('meta[name="description"]'))
      .toBe(metaContent('meta[property="og:description"]'));

    const script = document.getElementById('ld-event');
    expect(script).not.toBeNull();
    const data = JSON.parse(script!.textContent!);
    expect(data['@type']).toBe('Event');
    expect(data.name).toContain(GlobalConstants.editionNumber);
    expect(data.startDate).toMatch(new RegExp(`^${GlobalConstants.year}-\\d{2}-\\d{2}$`));
  });

  it('en una página interior actualiza la canónica y retira el JSON-LD del evento', async () => {
    await router.navigateByUrl('/');
    await router.navigateByUrl('/rules');

    expect(canonicalHref()).toBe(`${SITE_URL}/rules`);
    expect(metaContent('meta[property="og:url"]')).toBe(`${SITE_URL}/rules`);
    expect(document.getElementById('ld-event')).toBeNull();
    expect(document.getElementById('ld-organization')).not.toBeNull();
  });

  it('cada ruta publica su título y descripción', async () => {
    await router.navigateByUrl('/rules');
    expect(document.title).toContain('Bases');
    expect(metaContent('meta[name="description"]')).toContain('Bases');

    await router.navigateByUrl('/registration');
    expect(document.title).toContain('Inscripción');
    expect(metaContent('meta[name="description"]'))
      .toContain(GlobalConstants.startRegistrationDate);
  });

  it('preloadImage añade el preload una sola vez', () => {
    const service = TestBed.inject(SeoService);
    service.preloadImage('/assets/img/hero.webp');
    service.preloadImage('/assets/img/hero.webp');
    const links = document.head.querySelectorAll(
      'link[rel="preload"][as="image"][href="/assets/img/hero.webp"]',
    );
    expect(links.length).toBe(1);
  });
});
