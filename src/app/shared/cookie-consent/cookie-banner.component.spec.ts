import { ComponentFixture, TestBed } from '@angular/core/testing';
import { beforeEach, describe, expect, it } from 'vitest';

import { CookieBannerComponent } from './cookie-banner.component';

async function render(): Promise<ComponentFixture<CookieBannerComponent>> {
  const fixture = TestBed.createComponent(CookieBannerComponent);
  fixture.detectChanges();
  await fixture.whenStable();
  fixture.detectChanges();
  return fixture;
}

describe('CookieBannerComponent', () => {
  beforeEach(() => {
    localStorage.clear();
  });

  it('se muestra si el visitante aún no ha decidido', async () => {
    const fixture = await render();
    expect(fixture.nativeElement.querySelector('.cookie-banner')).not.toBeNull();
  });

  it('no se muestra si ya hay una decisión guardada', async () => {
    localStorage.setItem('cookie-consent', 'rejected');
    const fixture = await render();
    expect(fixture.nativeElement.querySelector('.cookie-banner')).toBeNull();
  });

  it('aceptar guarda la decisión y oculta el banner', async () => {
    const fixture = await render();
    fixture.nativeElement.querySelectorAll('button')[0].click();
    fixture.detectChanges();
    expect(localStorage.getItem('cookie-consent')).toBe('accepted');
    expect(fixture.nativeElement.querySelector('.cookie-banner')).toBeNull();
  });

  it('rechazar guarda la decisión y oculta el banner', async () => {
    const fixture = await render();
    fixture.nativeElement.querySelectorAll('button')[1].click();
    fixture.detectChanges();
    expect(localStorage.getItem('cookie-consent')).toBe('rejected');
    expect(fixture.nativeElement.querySelector('.cookie-banner')).toBeNull();
  });
});
