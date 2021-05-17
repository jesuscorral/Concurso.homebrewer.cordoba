import { Component } from '@angular/core';
import { GlobalConstants } from 'app/shared/global-constants';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  year = GlobalConstants.year;
  day = GlobalConstants.day;
  month = GlobalConstants.month;
}