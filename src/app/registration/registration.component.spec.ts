import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { afterEach, describe, expect, it, vi } from 'vitest';

import { GlobalConstants } from '../shared/global-constants';
import { parseSpanishDate } from '../shared/registration-window';
import { RegistrationComponent } from './registration.component';

async function renderAt(now: Date): Promise<HTMLElement> {
  vi.useFakeTimers({ toFake: ['Date'] });
  vi.setSystemTime(now);
  TestBed.configureTestingModule({ imports: [RegistrationComponent], providers: [provideRouter([])] });
  const fixture = TestBed.createComponent(RegistrationComponent);
  fixture.detectChanges();
  await fixture.whenStable();
  fixture.detectChanges();
  return fixture.nativeElement as HTMLElement;
}

describe('RegistrationComponent', () => {
  const start = parseSpanishDate(GlobalConstants.startRegistrationDate);
  const end = parseSpanishDate(GlobalConstants.endRegistrationDate);

  afterEach(() => {
    vi.useRealTimers();
  });

  it('muestra el formulario dentro del plazo', async () => {
    const el = await renderAt(new Date(start.getTime() + 60_000));
    expect(el.querySelector('iframe.registration-form')).not.toBeNull();
  });

  it('antes del plazo avisa de cuándo abre y oculta el formulario', async () => {
    const el = await renderAt(new Date(start.getTime() - 60_000));
    expect(el.querySelector('iframe')).toBeNull();
    expect(el.textContent).toContain('aún no está abierta');
    expect(el.textContent).toContain(GlobalConstants.startRegistrationDate);
  });

  it('después del plazo indica que ha finalizado y oculta el formulario', async () => {
    const afterEnd = new Date(end);
    afterEnd.setDate(afterEnd.getDate() + 2);
    const el = await renderAt(afterEnd);
    expect(el.querySelector('iframe')).toBeNull();
    expect(el.textContent).toContain('ha finalizado');
    expect(el.textContent).toContain(GlobalConstants.endRegistrationDate);
  });

  it('antes de comprobar la fecha muestra un estado neutro', () => {
    TestBed.configureTestingModule({ imports: [RegistrationComponent], providers: [provideRouter([])] });
    const fixture = TestBed.createComponent(RegistrationComponent);
    expect(fixture.componentInstance.status()).toBe('checking');
  });
});
