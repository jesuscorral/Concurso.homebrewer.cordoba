import { Component, afterNextRender, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { GlobalConstants } from '../shared/global-constants';
import { SeoService } from '../shared/seo/seo.service';
import { registrationStatus } from '../shared/registration-window';
import { SPECIAL_AWARDS } from '../awards/awards.data';
import { HOME_HAS_NEWS, NewsComponent } from './news/news.component';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    imports: [RouterLink, NewsComponent]
})
export class HomeComponent {
  year = GlobalConstants.year;
  day = GlobalConstants.day;
  month = GlobalConstants.month;
  editionNumber = GlobalConstants.editionNumber;
  startRegistrationDate = GlobalConstants.startRegistrationDate;
  endRegistrationDate = GlobalConstants.endRegistrationDate;
  startReceptionDate = GlobalConstants.startReceptionDate;
  endReceptionDate = GlobalConstants.endReceptionDate;

  registrationExtended = !!GlobalConstants.previousEndRegistrationDate;
  specialAwardTitles = SPECIAL_AWARDS.map(award => award.title);
  hasNews = HOME_HAS_NEWS;

  // La portada se prerenderiza en el build: si el plazo ya cerró, los avisos
  // de inscripción se ocultan en el navegador del visitante.
  registrationClosed = signal(false);

  constructor() {
    // La imagen hero es el LCP de la portada; precargarla adelanta su pintado.
    inject(SeoService).preloadImage('assets/img/homepage/bg.webp');
    afterNextRender(() => this.registrationClosed.set(registrationStatus(new Date()) === 'closed'));
  }
}
