import { Component } from '@angular/core';
import { GlobalConstants } from 'app/shared/global-constants';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss']
})
export class OrganizationComponent { 
  edition: string = GlobalConstants.editionNumber;
}
