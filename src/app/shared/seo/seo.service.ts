import { DOCUMENT } from '@angular/common';
import { Injectable, inject } from '@angular/core';
import { Meta, Title } from '@angular/platform-browser';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

import { GlobalConstants } from '../global-constants';

const SITE_URL = 'https://www.concursohomebrewercordoba.es';
const OG_IMAGE = `${SITE_URL}/assets/img/og-image.jpg`;
const ORGANIZER = 'The Real CordobALE - CerveCataClub';

const MONTH_NUMBERS: Record<string, string> = {
  enero: '01', febrero: '02', marzo: '03', abril: '04', mayo: '05', junio: '06',
  julio: '07', agosto: '08', septiembre: '09', octubre: '10', noviembre: '11', diciembre: '12',
};

/**
 * Mantiene los metadatos SEO sincronizados con la ruta activa: meta
 * description y Open Graph (desde el `data.description` de cada ruta),
 * URL canónica y datos estructurados JSON-LD. Durante el prerendering
 * todo queda incluido en el HTML estático de cada página.
 */
@Injectable({ providedIn: 'root' })
export class SeoService {
  private readonly router = inject(Router);
  private readonly meta = inject(Meta);
  private readonly title = inject(Title);
  private readonly document = inject(DOCUMENT);

  init(): void {
    this.addJsonLd('ld-organization', this.organizationJsonLd());
    this.router.events
      .pipe(filter((event): event is NavigationEnd => event instanceof NavigationEnd))
      .subscribe((event) => this.update(event.urlAfterRedirects));
    // Cubre el caso de que la navegación inicial termine antes de la suscripción.
    if (this.router.navigated) {
      this.update(this.router.url);
    }
  }

  /** Añade un preload de imagen (p. ej. la imagen hero, que es el LCP). */
  preloadImage(href: string): void {
    const links = this.document.head.querySelectorAll<HTMLLinkElement>('link[rel="preload"][as="image"]');
    if (Array.from(links).some((link) => link.getAttribute('href') === href)) {
      return;
    }
    const link = this.document.createElement('link');
    link.setAttribute('rel', 'preload');
    link.setAttribute('as', 'image');
    link.setAttribute('href', href);
    this.document.head.appendChild(link);
  }

  private update(url: string): void {
    const path = url.split('?')[0].split('#')[0];
    const canonicalUrl = path === '/' ? `${SITE_URL}/` : `${SITE_URL}${path}`;
    const route = this.deepestRoute();
    const description = route.data['description'];
    // El título se lee del snapshot: el TitleStrategy del router actualiza
    // document.title después de emitir NavigationEnd.
    const pageTitle = route.title ?? this.title.getTitle();

    if (description) {
      this.meta.updateTag({ name: 'description', content: description });
      this.meta.updateTag({ property: 'og:description', content: description });
    }
    this.meta.updateTag({ property: 'og:title', content: pageTitle });
    this.meta.updateTag({ property: 'og:url', content: canonicalUrl });
    this.setCanonical(canonicalUrl);

    // El marcado del evento solo va en la portada, que es la página que lo describe.
    if (path === '/') {
      this.addJsonLd('ld-event', this.eventJsonLd());
    } else {
      this.removeJsonLd('ld-event');
    }
  }

  private deepestRoute() {
    let route = this.router.routerState.snapshot.root;
    while (route.firstChild) {
      route = route.firstChild;
    }
    return route;
  }

  private setCanonical(href: string): void {
    let link = this.document.head.querySelector<HTMLLinkElement>('link[rel="canonical"]');
    if (!link) {
      link = this.document.createElement('link');
      link.setAttribute('rel', 'canonical');
      this.document.head.appendChild(link);
    }
    link.setAttribute('href', href);
  }

  private addJsonLd(id: string, data: object): void {
    this.removeJsonLd(id);
    const script = this.document.createElement('script');
    script.id = id;
    script.type = 'application/ld+json';
    script.text = JSON.stringify(data);
    this.document.head.appendChild(script);
  }

  private removeJsonLd(id: string): void {
    this.document.getElementById(id)?.remove();
  }

  private eventJsonLd(): object {
    const month = MONTH_NUMBERS[GlobalConstants.month.toLowerCase()] ?? '01';
    const day = GlobalConstants.day.padStart(2, '0');
    return {
      '@context': 'https://schema.org',
      '@type': 'Event',
      name: `${GlobalConstants.editionNumber} Concurso Homebrewer Córdoba`,
      description: `Concurso de cerveza casera de Córdoba organizado por ${ORGANIZER}. Certamen oficial BJCP.`,
      startDate: `${GlobalConstants.year}-${month}-${day}`,
      eventStatus: 'https://schema.org/EventScheduled',
      eventAttendanceMode: 'https://schema.org/OfflineEventAttendanceMode',
      location: {
        '@type': 'Place',
        name: 'Córdoba',
        address: { '@type': 'PostalAddress', addressLocality: 'Córdoba', addressRegion: 'Andalucía', addressCountry: 'ES' },
      },
      image: [OG_IMAGE],
      organizer: { '@type': 'Organization', name: ORGANIZER, url: SITE_URL },
      url: `${SITE_URL}/`,
    };
  }

  private organizationJsonLd(): object {
    return {
      '@context': 'https://schema.org',
      '@type': 'Organization',
      name: ORGANIZER,
      url: SITE_URL,
      logo: `${SITE_URL}/assets/img/homepage/logoConcurso.png`,
      email: 'homebrewercordoba@gmail.com',
      sameAs: ['https://www.instagram.com/the_real_cordobale/'],
    };
  }
}
