import { Component } from '@angular/core';
import { GlobalConstants } from '../shared/global-constants';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    standalone: false
})
export class HomeComponent {
  year = GlobalConstants.year;
  day = GlobalConstants.day;
  month = GlobalConstants.month;
  editionNumber = GlobalConstants.editionNumber;
}