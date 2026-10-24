/**
 * Premios aportados por los patrocinadores.
 *
 * Mientras la lista esté vacía, la página muestra el aviso de premios pendientes
 * de confirmar. Para publicar un premio, añade un objeto por categoría, p. ej.:
 *
 *   {
 *     title: 'Estilos clásicos',
 *     places: [
 *       { place: 1, prizes: [{ sponsor: 'Mr. Malt', description: 'Saco de 25 kg de malta Pilsner' }] },
 *       { place: 2, prizes: [{ sponsor: 'Install Beer', description: 'Vale de 30 €' }] },
 *     ],
 *   },
 */
export interface SponsorPrize {
  sponsor: string;
  description: string;
}

export interface AwardPlace {
  place: 1 | 2 | 3;
  prizes: SponsorPrize[];
}

export interface AwardCategory {
  title: string;
  places: AwardPlace[];
}

export const SPONSOR_AWARDS: AwardCategory[] = [];

/** Número de premios de patrocinadores y patrocinadores distintos que los aportan. */
export function summarizeSponsorAwards(categories: AwardCategory[]): { prizes: number; sponsors: string[] } {
  const prizes = categories.flatMap(category => category.places.flatMap(place => place.prizes));
  return { prizes: prizes.length, sponsors: [...new Set(prizes.map(prize => prize.sponsor))] };
}

/**
 * Premios especiales y regalos que no dependen del puesto en una categoría
 * (p. ej. "Rookie del Año"). Se muestran en la página de premios y en la portada.
 * `detail` es opcional: úsalo para indicar novedades pendientes ("Pronto más detalles").
 */
export interface SpecialAward {
  icon: string; // Clase de Font Awesome 4.7 sin el prefijo "fa-" (p. ej. 'star')
  title: string;
  description: string;
  sponsor?: string;
  sponsorUrl?: string;
  detail?: string;
}

export const SPECIAL_AWARDS: SpecialAward[] = [
  {
    icon: 'star',
    title: 'Rookie del Año',
    description:
      'Nuevo premio especial para animar a quienes están empezando: lo recibirá la cerveza mejor ' +
      'clasificada entre los cerveceros que participan por primera vez en nuestro concurso.',
  },
  {
    icon: 'gift',
    title: 'Regalo para todos los participantes',
    description:
      'Todos los participantes recibirán una invitación a un webinar exclusivo sobre elaboración ' +
      'de cervezas históricas.',
    sponsor: 'Libros de Guarda',
    sponsorUrl: 'https://es.librosdeguarda.com/',
    detail: 'Pronto facilitaremos todos los detalles.',
  },
];
