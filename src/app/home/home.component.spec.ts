import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { afterEach, describe, expect, it, vi } from 'vitest';

import { SPECIAL_AWARDS } from '../awards/awards.data';
import { GlobalConstants } from '../shared/global-constants';
import { parseSpanishDate } from '../shared/registration-window';
import { HomeComponent } from './home.component';

async function renderAt(now: Date): Promise<HTMLElement> {
  vi.useFakeTimers({ toFake: ['Date'] });
  vi.setSystemTime(now);
  TestBed.configureTestingModule({ imports: [HomeComponent], providers: [provideRouter([])] });
  const fixture = TestBed.createComponent(HomeComponent);
  fixture.detectChanges();
  await fixture.whenStable();
  fixture.detectChanges();
  return fixture.nativeElement as HTMLElement;
}

describe('HomeComponent', () => {
  const end = parseSpanishDate(GlobalConstants.endRegistrationDate);

  afterEach(() => {
    vi.useRealTimers();
  });

  it('muestra el título de la edición y la fecha del concurso', async () => {
    const el = await renderAt(end);
    expect(el.querySelector('h1')?.textContent).toContain(GlobalConstants.editionNumber);
    expect(el.querySelector('.hero-date')?.textContent).toContain(GlobalConstants.month);
  });

  it('durante el plazo muestra el aviso de plazo ampliado y el de nuevos premios', async () => {
    const el = await renderAt(end);
    const alerts = el.querySelectorAll('.hero-alert');
    expect(alerts.length).toBe(2);
    expect(alerts[0].textContent).toContain('Plazo ampliado');
    expect(alerts[0].textContent).toContain(GlobalConstants.endRegistrationDate);
    expect(alerts[1].getAttribute('href')).toBe('/awards');
    for (const award of SPECIAL_AWARDS) {
      expect(alerts[1].textContent).toContain(award.title);
    }
  });

  it('cuando cierra el plazo oculta los avisos de inscripción', async () => {
    const afterEnd = new Date(end);
    afterEnd.setDate(afterEnd.getDate() + 2);
    const el = await renderAt(afterEnd);
    expect(el.textContent).not.toContain('Plazo ampliado');
    expect(el.querySelector('.news-card--urgent')).toBeNull();
    // Los premios siguen anunciándose.
    expect(el.querySelector('.hero-alert--awards')).not.toBeNull();
    expect(el.querySelector('.news-awards')).not.toBeNull();
  });

  it('la flecha del hero baja a la sección de novedades', async () => {
    const el = await renderAt(end);
    expect(el.querySelector('.scroll-cue')?.getAttribute('href')).toBe('#novedades');
    expect(el.querySelector('#novedades')).not.toBeNull();
  });
});
