import { PLATFORM_ID } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';

import { CookieConsentService } from './cookie-consent.service';

type WindowWithGtag = Window & { gtag?: (...args: unknown[]) => void };

describe('CookieConsentService (navegador)', () => {
  let service: CookieConsentService;
  let gtag: ReturnType<typeof vi.fn<(...args: unknown[]) => void>>;

  beforeEach(() => {
    localStorage.clear();
    gtag = vi.fn<(...args: unknown[]) => void>();
    (window as WindowWithGtag).gtag = gtag;
    service = TestBed.inject(CookieConsentService);
  });

  afterEach(() => {
    delete (window as WindowWithGtag).gtag;
  });

  it('sin decisión previa el estado es null', () => {
    expect(service.status).toBeNull();
  });

  it('un valor corrupto en localStorage se trata como null', () => {
    localStorage.setItem('cookie-consent', 'cualquier-cosa');
    expect(service.status).toBeNull();
  });

  it('accept() recuerda la decisión y concede el consentimiento de análisis', () => {
    service.accept();
    expect(service.status).toBe('accepted');
    expect(gtag).toHaveBeenCalledWith('consent', 'update', { analytics_storage: 'granted' });
  });

  it('reject() recuerda la decisión y deniega el consentimiento de análisis', () => {
    service.reject();
    expect(service.status).toBe('rejected');
    expect(gtag).toHaveBeenCalledWith('consent', 'update', { analytics_storage: 'denied' });
  });

  it('no falla si gtag.js no está disponible (p. ej. bloqueado por el navegador)', () => {
    delete (window as WindowWithGtag).gtag;
    expect(() => service.accept()).not.toThrow();
    expect(service.status).toBe('accepted');
  });
});

describe('CookieConsentService (servidor / prerender)', () => {
  it('no toca localStorage fuera del navegador', () => {
    TestBed.configureTestingModule({
      providers: [{ provide: PLATFORM_ID, useValue: 'server' }],
    });
    const service = TestBed.inject(CookieConsentService);

    localStorage.setItem('cookie-consent', 'accepted');
    expect(service.status).toBeNull();
    localStorage.clear();
  });
});
