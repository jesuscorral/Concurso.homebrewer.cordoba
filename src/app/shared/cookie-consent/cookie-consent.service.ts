import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

export type ConsentStatus = 'accepted' | 'rejected' | null;

type Gtag = (command: 'consent', action: 'update', params: Record<string, 'granted' | 'denied'>) => void;

/**
 * Gestiona el consentimiento de cookies (RGPD). La etiqueta de Google Analytics
 * (gtag.js) se carga en index.html con Consent Mode v2 en "denied"; este servicio
 * recuerda la decisión en localStorage y concede o retira el permiso de análisis.
 */
@Injectable({ providedIn: 'root' })
export class CookieConsentService {
  private static readonly storageKey = 'cookie-consent';

  private readonly isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  get status(): ConsentStatus {
    if (!this.isBrowser) {
      return null;
    }
    const value = localStorage.getItem(CookieConsentService.storageKey);
    return value === 'accepted' || value === 'rejected' ? value : null;
  }

  accept(): void {
    localStorage.setItem(CookieConsentService.storageKey, 'accepted');
    this.updateConsent('granted');
  }

  reject(): void {
    localStorage.setItem(CookieConsentService.storageKey, 'rejected');
    this.updateConsent('denied');
  }

  private updateConsent(analyticsStorage: 'granted' | 'denied'): void {
    const gtag = (window as unknown as { gtag?: Gtag }).gtag;
    gtag?.('consent', 'update', { analytics_storage: analyticsStorage });
  }
}
