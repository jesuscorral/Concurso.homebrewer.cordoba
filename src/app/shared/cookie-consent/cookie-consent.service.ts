import { Injectable } from '@angular/core';

export type ConsentStatus = 'accepted' | 'rejected' | null;

/**
 * Gestiona el consentimiento de cookies (RGPD): Google Tag Manager solo se
 * carga si el visitante acepta, y la decisión se recuerda en localStorage.
 */
@Injectable({ providedIn: 'root' })
export class CookieConsentService {
  private static readonly storageKey = 'cookie-consent';
  private static readonly gtmId = 'GTM-PCDV79NT';

  get status(): ConsentStatus {
    const value = localStorage.getItem(CookieConsentService.storageKey);
    return value === 'accepted' || value === 'rejected' ? value : null;
  }

  init(): void {
    if (this.status === 'accepted') {
      this.loadGtm();
    }
  }

  accept(): void {
    localStorage.setItem(CookieConsentService.storageKey, 'accepted');
    this.loadGtm();
  }

  reject(): void {
    localStorage.setItem(CookieConsentService.storageKey, 'rejected');
  }

  private loadGtm(): void {
    if (document.getElementById('gtm-script')) {
      return;
    }
    const w = window as unknown as { dataLayer: unknown[] };
    w.dataLayer = w.dataLayer || [];
    w.dataLayer.push({ 'gtm.start': new Date().getTime(), event: 'gtm.js' });
    const script = document.createElement('script');
    script.id = 'gtm-script';
    script.async = true;
    script.src = `https://www.googletagmanager.com/gtm.js?id=${CookieConsentService.gtmId}`;
    document.head.appendChild(script);
  }
}
