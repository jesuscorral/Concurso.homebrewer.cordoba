import { Component, afterNextRender, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { GlobalConstants } from '../shared/global-constants';
import { RegistrationStatus, registrationStatus } from '../shared/registration-window';

@Component({
    selector: 'app-registration',
    imports: [RouterLink],
    templateUrl: './registration.component.html',
    styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent {
  startRegistrationDate = GlobalConstants.startRegistrationDate;
  endRegistrationDate = GlobalConstants.endRegistrationDate;
  year = GlobalConstants.year;

  // La página se prerenderiza en el build: la fecha solo se comprueba en el
  // navegador del visitante. Mientras tanto se muestra un estado neutro.
  status = signal<RegistrationStatus | 'checking'>('checking');

  constructor() {
    afterNextRender(() => this.status.set(registrationStatus(new Date())));
  }
}
