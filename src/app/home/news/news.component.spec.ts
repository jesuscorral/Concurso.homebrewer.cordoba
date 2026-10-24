import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { beforeEach, describe, expect, it } from 'vitest';

import { SPECIAL_AWARDS } from '../../awards/awards.data';
import { GlobalConstants } from '../../shared/global-constants';
import { NewsComponent } from './news.component';

describe('NewsComponent', () => {
  let fixture: ComponentFixture<NewsComponent>;
  let el: HTMLElement;

  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [NewsComponent], providers: [provideRouter([])] });
    fixture = TestBed.createComponent(NewsComponent);
    el = fixture.nativeElement as HTMLElement;
  });

  it('lista los premios especiales con un enlace a la página de premios', () => {
    fixture.detectChanges();
    const items = el.querySelectorAll('.news-awards-list li');
    expect(items.length).toBe(SPECIAL_AWARDS.length);
    SPECIAL_AWARDS.forEach((award, i) => expect(items[i].textContent).toContain(award.title));
    expect(el.querySelector('.news-cta')?.getAttribute('href')).toBe('/awards');
  });

  it('muestra el patrocinador del premio cuando lo tiene', () => {
    fixture.detectChanges();
    const sponsored = SPECIAL_AWARDS.find((award) => award.sponsor);
    expect(sponsored).toBeDefined();
    expect(el.querySelector('.news-sponsor')?.textContent).toContain(sponsored!.sponsor!);
  });

  it('con el plazo abierto anuncia la ampliación y el cambio en las bases', () => {
    fixture.detectChanges();
    const urgent = el.querySelector('.news-card--urgent');
    expect(urgent?.textContent).toContain(GlobalConstants.endRegistrationDate);
    expect(urgent?.querySelector('s')?.textContent).toBe(GlobalConstants.previousEndRegistrationDate);
    expect(el.textContent).toContain('3 cervezas en la misma categoría');
  });

  it('con el plazo cerrado oculta las tarjetas de inscripción', () => {
    fixture.componentRef.setInput('registrationClosed', true);
    fixture.detectChanges();
    expect(el.querySelector('.news-grid')).toBeNull();
    expect(el.querySelector('.news-awards')).not.toBeNull();
  });

  it('cuenta un premio de patrocinador confirmado en singular', () => {
    fixture.componentInstance.confirmedPrizes = 1;
    fixture.componentInstance.prizeSponsors = ['Mr. Malt'];
    fixture.detectChanges();
    expect(el.textContent).toContain('1 premio confirmado de los patrocinadores');
    expect(el.textContent).toContain('Gracias a Mr. Malt.');
  });

  it('cuenta varios premios de patrocinadores en plural', () => {
    fixture.componentInstance.confirmedPrizes = 3;
    fixture.componentInstance.prizeSponsors = ['Mr. Malt', 'Install Beer'];
    fixture.detectChanges();
    expect(el.textContent).toContain('3 premios confirmados de los patrocinadores');
    expect(el.textContent).toContain('Gracias a Mr. Malt, Install Beer.');
  });
});
