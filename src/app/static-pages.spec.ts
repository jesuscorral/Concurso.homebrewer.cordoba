import { Type } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { describe, expect, it } from 'vitest';

import { ContactComponent } from './contact/contact.component';
import { OrganizationComponent } from './organization/organization.component';
import { SponsorsComponent } from './sponsors/sponsors.component';
import { FooterComponent } from './shared/footer/footer.component';

// Páginas sin lógica: basta con comprobar que se renderizan con su contenido clave.
const pages: { name: string; component: Type<unknown>; expected: string }[] = [
  { name: 'Contacto', component: ContactComponent, expected: 'homebrewercordoba@gmail.com' },
  { name: 'Organización', component: OrganizationComponent, expected: 'CordobALE' },
  { name: 'Patrocinadores', component: SponsorsComponent, expected: 'Libros de Guarda' },
];

describe('Páginas estáticas', () => {
  for (const { name, component, expected } of pages) {
    it(`${name} se renderiza con su contenido`, () => {
      TestBed.configureTestingModule({ imports: [component], providers: [provideRouter([])] });
      const fixture = TestBed.createComponent(component);
      fixture.detectChanges();
      expect((fixture.nativeElement as HTMLElement).textContent).toContain(expected);
    });
  }

  it('el pie muestra el año actual', () => {
    TestBed.configureTestingModule({ imports: [FooterComponent] });
    const fixture = TestBed.createComponent(FooterComponent);
    fixture.detectChanges();
    expect((fixture.nativeElement as HTMLElement).textContent).toContain(String(new Date().getFullYear()));
  });
});
