import { PLATFORM_ID } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { beforeEach, describe, expect, it } from 'vitest';

import { CookieConsentService } from './cookie-consent.service';

describe('CookieConsentService (navegador)', () => {
  let service: CookieConsentService;

  beforeEach(() => {
    localStorage.clear();
    document.getElementById('gtm-script')?.remove();
    service = TestBed.inject(CookieConsentService);
  });

  it('sin decisión previa el estado es null y no se carga GTM', () => {
    expect(service.status).toBeNull();
    service.init();
    expect(document.getElementById('gtm-script')).toBeNull();
  });

  it('un valor corrupto en localStorage se trata como null', () => {
    localStorage.setItem('cookie-consent', 'cualquier-cosa');
    expect(service.status).toBeNull();
  });

  it('accept() recuerda la decisión y carga GTM', () => {
    service.accept();
    expect(service.status).toBe('accepted');
    expect(document.getElementById('gtm-script')).not.toBeNull();
  });

  it('reject() recuerda la decisión y no carga GTM', () => {
    service.reject();
    expect(service.status).toBe('rejected');
    expect(document.getElementById('gtm-script')).toBeNull();
  });

  it('init() carga GTM si la visita anterior aceptó', () => {
    localStorage.setItem('cookie-consent', 'accepted');
    service.init();
    expect(document.getElementById('gtm-script')).not.toBeNull();
  });
});

describe('CookieConsentService (servidor / prerender)', () => {
  it('no toca localStorage ni el DOM fuera del navegador', () => {
    document.getElementById('gtm-script')?.remove();
    TestBed.configureTestingModule({
      providers: [{ provide: PLATFORM_ID, useValue: 'server' }],
    });
    const service = TestBed.inject(CookieConsentService);

    localStorage.setItem('cookie-consent', 'accepted');
    expect(service.status).toBeNull();
    service.init();
    expect(document.getElementById('gtm-script')).toBeNull();
    localStorage.clear();
  });
});
