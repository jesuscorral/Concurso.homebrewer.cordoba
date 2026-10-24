import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { beforeEach, describe, expect, it } from 'vitest';

import { AwardsComponent } from './awards.component';
import { SPECIAL_AWARDS } from './awards.data';

describe('AwardsComponent', () => {
  let fixture: ComponentFixture<AwardsComponent>;
  let el: HTMLElement;

  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [AwardsComponent], providers: [provideRouter([])] });
    fixture = TestBed.createComponent(AwardsComponent);
    el = fixture.nativeElement as HTMLElement;
  });

  it('muestra una tarjeta por premio especial', () => {
    fixture.detectChanges();
    const cards = el.querySelectorAll('.special-card');
    expect(cards.length).toBe(SPECIAL_AWARDS.length);
    SPECIAL_AWARDS.forEach((award, i) => {
      expect(cards[i].querySelector('h3')?.textContent).toBe(award.title);
      expect(cards[i].querySelector(`.fa-${award.icon}`)).not.toBeNull();
    });
  });

  it('enlaza al patrocinador y muestra el detalle pendiente', () => {
    fixture.detectChanges();
    const sponsored = SPECIAL_AWARDS.find((award) => award.sponsorUrl)!;
    const link = el.querySelector<HTMLAnchorElement>('.special-sponsor a');
    expect(link?.textContent).toBe(sponsored.sponsor);
    expect(link?.getAttribute('href')).toBe(sponsored.sponsorUrl);
    expect(el.querySelector('.special-detail')?.textContent).toBe(sponsored.detail);
  });

  it('muestra el patrocinador sin enlace cuando no hay web', () => {
    fixture.componentInstance.specialAwards = [
      { icon: 'gift', title: 'Lote', description: 'Un lote de lúpulo', sponsor: 'Tienda' },
    ];
    fixture.detectChanges();
    const sponsor = el.querySelector('.special-sponsor');
    expect(sponsor?.textContent).toContain('Tienda');
    expect(sponsor?.querySelector('a')).toBeNull();
  });

  it('sin premios de patrocinadores avisa de que faltan por confirmar', () => {
    fixture.componentInstance.categories = [];
    fixture.detectChanges();
    expect(el.querySelector('.pending-card')?.textContent).toContain('aún no están todos confirmados');
  });

  it('pinta los premios de patrocinadores por categoría y puesto', () => {
    fixture.componentInstance.categories = [
      {
        title: 'Estilos clásicos',
        places: [
          { place: 1, prizes: [{ sponsor: 'Mr. Malt', description: 'Saco de malta' }] },
          { place: 2, prizes: [{ sponsor: 'Install Beer', description: 'Vale de 30 €' }] },
        ],
      },
    ];
    fixture.detectChanges();
    expect(el.querySelector('.pending-card')).toBeNull();
    expect(el.querySelector('.category-card h3')?.textContent).toBe('Estilos clásicos');
    const rows = el.querySelectorAll('.place-row');
    expect(rows.length).toBe(2);
    expect(rows[0].querySelector('img')?.getAttribute('src')).toContain('PrimerPremio');
    expect(rows[1].textContent).toContain('Install Beer');
    expect(el.querySelector('.more-coming')?.textContent).toContain('Aún no están todos los premios confirmados');
  });

  it('oculta la sección de premios especiales si no hay ninguno', () => {
    fixture.componentInstance.specialAwards = [];
    fixture.detectChanges();
    expect(el.querySelector('.special-grid')).toBeNull();
  });
});
