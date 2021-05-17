import { Component } from '@angular/core';
import { GlobalConstants } from 'app/shared/global-constants';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss']
})
export class ContactComponent  {
  edition: string = GlobalConstants.editionNumber;
}
