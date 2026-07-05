import { DOCUMENT } from '@angular/common';
import { Component, HostListener, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.scss'],
    imports: [RouterLink, RouterLinkActive]
})
export class NavbarComponent {
  private readonly document = inject(DOCUMENT);

  scrolled = false;
  readonly menuOpen = signal(false);

  @HostListener('window:scroll')
  onWindowScroll(): void {
    this.scrolled = window.scrollY > 60;
  }

  @HostListener('document:keydown.escape')
  onEscape(): void {
    this.closeMenu();
  }

  toggleMenu(): void {
    this.menuOpen.update((open) => !open);
    this.syncBodyScroll();
  }

  closeMenu(): void {
    if (this.menuOpen()) {
      this.menuOpen.set(false);
      this.syncBodyScroll();
    }
  }

  /** Bloquea el scroll de fondo mientras el drawer móvil está abierto. */
  private syncBodyScroll(): void {
    this.document.body.style.overflow = this.menuOpen() ? 'hidden' : '';
  }
}
