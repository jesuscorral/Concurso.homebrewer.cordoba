import { Component } from '@angular/core';
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
}
