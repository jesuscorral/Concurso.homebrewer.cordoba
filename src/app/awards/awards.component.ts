import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { GlobalConstants } from '../shared/global-constants';
import { SPONSOR_AWARDS } from './awards.data';

@Component({
    selector: 'app-awards',
    imports: [RouterLink],
    templateUrl: './awards.component.html',
    styleUrls: ['./awards.component.scss']
})
export class AwardsComponent {
  edition: string = GlobalConstants.editionNumber;
  categories = SPONSOR_AWARDS;
  placeImages: Record<number, string> = {
    1: 'assets/img/awards/PrimerPremio-Blanco.png',
    2: 'assets/img/awards/SegundoPremio-Blanco.png',
    3: 'assets/img/awards/TercerPremio-Blanco.png',
  };
}
