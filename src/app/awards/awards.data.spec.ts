import { describe, expect, it } from 'vitest';

import { SPECIAL_AWARDS, summarizeSponsorAwards } from './awards.data';

describe('awards.data', () => {
  it('sin categorías no hay premios ni patrocinadores', () => {
    expect(summarizeSponsorAwards([])).toEqual({ prizes: 0, sponsors: [] });
  });

  it('cuenta todos los premios y lista cada patrocinador una sola vez', () => {
    const summary = summarizeSponsorAwards([
      {
        title: 'Estilos clásicos',
        places: [
          { place: 1, prizes: [{ sponsor: 'Mr. Malt', description: 'Saco de malta' }, { sponsor: 'Install Beer', description: 'Vale' }] },
          { place: 2, prizes: [{ sponsor: 'Mr. Malt', description: 'Lúpulo' }] },
        ],
      },
      { title: 'Históricas', places: [{ place: 1, prizes: [{ sponsor: 'Libros de Guarda', description: 'Libro' }] }] },
    ]);
    expect(summary.prizes).toBe(4);
    expect(summary.sponsors).toEqual(['Mr. Malt', 'Install Beer', 'Libros de Guarda']);
  });

  it('los premios especiales tienen icono, título y descripción', () => {
    for (const award of SPECIAL_AWARDS) {
      expect(award.icon).toMatch(/^[a-z-]+$/);
      expect(award.title).toBeTruthy();
      expect(award.description).toBeTruthy();
    }
  });
});
