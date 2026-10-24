import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { describe, expect, it, vi } from 'vitest';

import { AppComponent } from './app.component';
import { SeoService } from './shared/seo/seo.service';

describe('AppComponent', () => {
  it('monta la estructura de la web e inicia el SEO', () => {
    TestBed.configureTestingModule({ imports: [AppComponent], providers: [provideRouter([])] });
    const init = vi.spyOn(TestBed.inject(SeoService), 'init').mockImplementation(() => undefined);

    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();

    const el = fixture.nativeElement as HTMLElement;
    expect(el.querySelector('app-navbar')).not.toBeNull();
    expect(el.querySelector('router-outlet')).not.toBeNull();
    expect(el.querySelector('app-footer')).not.toBeNull();
    expect(el.querySelector('app-cookie-banner')).not.toBeNull();
    expect(init).toHaveBeenCalledOnce();
  });
});
