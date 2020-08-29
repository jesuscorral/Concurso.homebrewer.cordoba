import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app.routing';

import { AppComponent } from './app.component';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { FooterComponent } from './shared/footer/footer.component';

import { RulesComponent } from './rules/rules.component';
import { RegistrationComponent } from './registration/registration.component';
import { OrganizationComponent } from './organization/organization.component';
import { ContactComponent } from './contact/contact.component';
import { HomeComponent } from './home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './shared/material/material-module';
import { AwardsComponent } from './awards/awards.component';
import { SponsorsComponent } from './sponsors/sponsors.component';


@NgModule({
  declarations: [
    HomeComponent,
    AppComponent,
    NavbarComponent,
    FooterComponent,
    RulesComponent,
    RegistrationComponent,
    OrganizationComponent,
    ContactComponent,
    AwardsComponent,
    SponsorsComponent
  ],
  imports: [
    BrowserModule,
    NgbModule,
    FormsModule,
    RouterModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
