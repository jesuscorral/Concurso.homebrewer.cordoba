import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { GlobalConstants } from '../../shared/global-constants';
import { SPECIAL_AWARDS, SPONSOR_AWARDS } from '../../awards/awards.data';

/** Sección "Últimas novedades" de la portada. Cada tarjeta se activa sola según los datos. */
@Component({
    selector: 'app-home-news',
    imports: [RouterLink],
    templateUrl: './news.component.html',
    styleUrls: ['./news.component.css']
})
export class NewsComponent {
  /** Oculta las tarjetas de inscripción cuando el plazo ya ha cerrado. */
  registrationClosed = input(false);

  year = GlobalConstants.year;
  endRegistrationDate = GlobalConstants.endRegistrationDate;
  previousEndRegistrationDate = GlobalConstants.previousEndRegistrationDate;
  startReceptionDate = GlobalConstants.startReceptionDate;
  endReceptionDate = GlobalConstants.endReceptionDate;

  registrationExtended = !!GlobalConstants.previousEndRegistrationDate;
  specialAwards = SPECIAL_AWARDS;
  confirmedPrizes = SPONSOR_AWARDS.reduce(
    (total, category) => total + category.places.reduce((sum, place) => sum + place.prizes.length, 0),
    0,
  );
  prizeSponsors = [...new Set(
    SPONSOR_AWARDS.flatMap(category => category.places.flatMap(place => place.prizes.map(prize => prize.sponsor))),
  )];
  hasAwardsNews = this.specialAwards.length > 0 || this.confirmedPrizes > 0;
}

/** True si la sección tiene algo que mostrar (para decidir el destino de la flecha del hero). */
export const HOME_HAS_NEWS =
  !!GlobalConstants.previousEndRegistrationDate || SPECIAL_AWARDS.length > 0 || SPONSOR_AWARDS.length > 0;
