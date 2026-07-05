import { Component, inject } from '@angular/core';
import { CookieConsentService } from './cookie-consent.service';

@Component({
    selector: 'app-cookie-banner',
    templateUrl: './cookie-banner.component.html',
    styleUrls: ['./cookie-banner.component.scss']
})
export class CookieBannerComponent {
  private readonly consent = inject(CookieConsentService);

  visible = this.consent.status === null;

  accept(): void {
    this.consent.accept();
    this.visible = false;
  }

  reject(): void {
    this.consent.reject();
    this.visible = false;
  }
}
