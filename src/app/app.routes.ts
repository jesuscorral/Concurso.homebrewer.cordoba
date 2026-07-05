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
    {
        path: '',
        component: HomeComponent,
        title: `${siteName} ${C.year} | Concurso de cerveza casera`,
        data: { description: `Concurso de cerveza casera de Córdoba, ${C.day} de ${C.month} de ${C.year}. Certamen oficial BJCP organizado por The Real CordobALE: bases, inscripción y premios.` }
    },
    // Compatibilidad con la URL antigua de la portada.
    { path: 'home', redirectTo: '', pathMatch: 'full' },
    {
        path: 'rules',
        component: RulesComponent,
        title: `Bases | ${siteName}`,
        data: { description: `Bases del ${C.editionNumber} Concurso Homebrewer Córdoba ${C.year}: estilos BJCP admitidos, formato de las botellas, plazos y condiciones de participación.` }
    },
    {
        path: 'sponsors',
        component: SponsorsComponent,
        title: `Patrocinadores | ${siteName}`,
        data: { description: 'Tiendas, marcas y locales cerveceros que patrocinan el Concurso Homebrewer Córdoba y aportan los premios del certamen.' }
    },
    {
        path: 'registration',
        component: RegistrationComponent,
        title: `Inscripción | ${siteName}`,
        data: { description: `Inscripción al ${C.editionNumber} Concurso Homebrewer Córdoba: del ${C.startRegistrationDate} al ${C.endRegistrationDate}. Recepción de botellas del ${C.startReceptionDate} al ${C.endReceptionDate}.` }
    },
    {
        path: 'organization',
        component: OrganizationComponent,
        title: `Organización | ${siteName}`,
        data: { description: 'The Real CordobALE - CerveCataClub, club de cata cervecera de Córdoba fundado en 2018 y organizador del Concurso Homebrewer Córdoba.' }
    },
    {
        path: 'contact',
        component: ContactComponent,
        title: `Contacto | ${siteName}`,
        data: { description: 'Contacta con la organización del Concurso Homebrewer Córdoba para dudas sobre las bases, la inscripción o el envío de botellas.' }
    },
    { path: '**', redirectTo: '' }
];
