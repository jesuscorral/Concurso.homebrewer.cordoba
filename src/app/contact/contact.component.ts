import { Component } from '@angular/core';
import { GlobalConstants } from '../shared/global-constants';

@Component({
    selector: 'app-contact',
    templateUrl: './contact.component.html',
    styleUrls: ['./contact.component.scss'],
    standalone: false
})
export class ContactComponent  {
  edition: string = GlobalConstants.editionNumber;
}
