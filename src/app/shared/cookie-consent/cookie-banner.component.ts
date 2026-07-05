import { Component, afterNextRender, inject, signal } from '@angular/core';
import { CookieConsentService } from './cookie-consent.service';

@Component({
    selector: 'app-cookie-banner',
    templateUrl: './cookie-banner.component.html',
    styleUrls: ['./cookie-banner.component.scss']
})
export class CookieBannerComponent {
  private readonly consent = inject(CookieConsentService);

  // Se decide tras el primer render en el navegador: el banner no forma parte
  // del HTML prerenderizado y no provoca desajustes de hidratación.
  readonly visible = signal(false);

  constructor() {
    afterNextRender(() => {
      this.visible.set(this.consent.status === null);
    });
  }

  accept(): void {
    this.consent.accept();
    this.visible.set(false);
  }

  reject(): void {
    this.consent.reject();
    this.visible.set(false);
  }
}
