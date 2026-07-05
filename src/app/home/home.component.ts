import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { GlobalConstants } from '../shared/global-constants';
import { SeoService } from '../shared/seo/seo.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    imports: [RouterLink]
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

  constructor() {
    // La imagen hero es el LCP de la portada; precargarla adelanta su pintado.
    inject(SeoService).preloadImage('assets/img/homepage/bg.webp');
  }
}
