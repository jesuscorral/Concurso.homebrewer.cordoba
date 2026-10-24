import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { afterEach, beforeEach, describe, expect, it } from 'vitest';

import { NavbarComponent } from './navbar.component';

describe('NavbarComponent', () => {
  let fixture: ComponentFixture<NavbarComponent>;
  let component: NavbarComponent;
  let el: HTMLElement;

  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [NavbarComponent], providers: [provideRouter([])] });
    fixture = TestBed.createComponent(NavbarComponent);
    component = fixture.componentInstance;
    el = fixture.nativeElement as HTMLElement;
    fixture.detectChanges();
  });

  afterEach(() => {
    document.body.style.overflow = '';
    Object.defineProperty(window, 'scrollY', { value: 0, configurable: true });
  });

  it('incluye un enlace por cada sección', () => {
    const links = Array.from(el.querySelectorAll('.nav-links a[href^="/"]')).map((a) => a.getAttribute('href'));
    expect(links).toEqual(['/rules', '/sponsors', '/awards', '/registration', '/organization', '/contact']);
  });

  it('el botón abre y cierra el menú móvil y bloquea el scroll de fondo', () => {
    const toggle = el.querySelector<HTMLButtonElement>('.nav-toggle')!;

    toggle.click();
    fixture.detectChanges();
    expect(component.menuOpen()).toBe(true);
    expect(toggle.getAttribute('aria-expanded')).toBe('true');
    expect(el.querySelector('nav')?.classList).toContain('menu-open');
    expect(document.body.style.overflow).toBe('hidden');

    toggle.click();
    fixture.detectChanges();
    expect(component.menuOpen()).toBe(false);
    expect(document.body.style.overflow).toBe('');
  });

  it('Escape y el fondo oscuro cierran el menú', () => {
    component.toggleMenu();
    document.dispatchEvent(new KeyboardEvent('keydown', { key: 'Escape' }));
    expect(component.menuOpen()).toBe(false);

    component.toggleMenu();
    fixture.detectChanges();
    el.querySelector<HTMLElement>('.nav-backdrop')!.click();
    expect(component.menuOpen()).toBe(false);
  });

  it('cerrar con el menú ya cerrado no toca el scroll', () => {
    document.body.style.overflow = 'auto';
    component.closeMenu();
    expect(document.body.style.overflow).toBe('auto');
  });

  it('marca la barra como "scrolled" al bajar más de 60px', () => {
    Object.defineProperty(window, 'scrollY', { value: 120, configurable: true });
    window.dispatchEvent(new Event('scroll'));
    expect(component.scrolled).toBe(true);

    Object.defineProperty(window, 'scrollY', { value: 10, configurable: true });
    window.dispatchEvent(new Event('scroll'));
    expect(component.scrolled).toBe(false);
  });
});
