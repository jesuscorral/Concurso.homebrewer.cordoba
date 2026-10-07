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
