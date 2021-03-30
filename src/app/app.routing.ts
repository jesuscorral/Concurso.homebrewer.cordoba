import { NgModule } from '@angular/core';
import { CommonModule, } from '@angular/common';
import { BrowserModule  } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { RulesComponent } from './rules/rules.component';
import { RegistrationComponent } from './registration/registration.component';
import { OrganizationComponent } from './organization/organization.component';
import { ContactComponent } from './contact/contact.component';
import { AwardsComponent } from './awards/awards.component';
import { SponsorsComponent } from './sponsors/sponsors.component';

const routes: Routes =[
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home',             component: HomeComponent },
    { path: 'rules',            component: RulesComponent },
    { path: 'awards',           component: AwardsComponent },
    { path: 'sponsors',         component: SponsorsComponent },
    { path: 'registration',     component: RegistrationComponent },
    { path: 'organization',     component: OrganizationComponent },
    { path: 'contact',          component: ContactComponent }
    //   // Other routes...
    //  { path: '**',  component: NotfoundComponent }
  ];

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule.forRoot(routes,{
    useHash: true,
    relativeLinkResolution: 'legacy'
})
  ],
  exports: [
  ],
})
export class AppRoutingModule { }
