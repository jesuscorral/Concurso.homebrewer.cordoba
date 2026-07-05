import { Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { RulesComponent } from './rules/rules.component';
import { RegistrationComponent } from './registration/registration.component';
import { OrganizationComponent } from './organization/organization.component';
import { ContactComponent } from './contact/contact.component';
import { SponsorsComponent } from './sponsors/sponsors.component';
import { GlobalConstants as C } from './shared/global-constants';

const siteName = 'Concurso Homebrewer Córdoba';

export const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home',             component: HomeComponent,          title: 'Concurso Homebrewer Córdoba' },
    { path: 'rules',            component: RulesComponent,         title: 'Bases | Concurso Homebrewer Córdoba' },
    { path: 'sponsors',         component: SponsorsComponent,      title: 'Patrocinadores | Concurso Homebrewer Córdoba' },
    { path: 'registration',     component: RegistrationComponent,  title: 'Inscripción | Concurso Homebrewer Córdoba' },
    { path: 'organization',     component: OrganizationComponent,  title: 'Organización | Concurso Homebrewer Córdoba' },
    { path: 'contact',          component: ContactComponent,       title: 'Contacto | Concurso Homebrewer Córdoba' },
    { path: '**', redirectTo: 'home' }
];
