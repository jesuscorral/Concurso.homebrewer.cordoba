import { describe, expect, it } from 'vitest';

import { GlobalConstants } from '../shared/global-constants';
import { RulesService } from './rules.service';

describe('RulesService', () => {
  const rules = new RulesService().get();

  it('devuelve las bases del concurso', () => {
    expect(rules.length).toBeGreaterThan(0);
  });

  it('todas las reglas tienen icono, título y contenido', () => {
    for (const rule of rules) {
      expect(rule.icon, `icono vacío en "${rule.title}"`).toBeTruthy();
      expect(rule.title).toBeTruthy();
      expect(rule.content, `contenido vacío en "${rule.title}"`).toBeTruthy();
    }
  });

  it('la fecha de celebración sale de GlobalConstants', () => {
    const rule = rules.find((r) => r.title.includes('Fecha y lugar'));
    expect(rule).toBeDefined();
    expect(rule!.content).toContain(
      `${GlobalConstants.day} de ${GlobalConstants.month} de ${GlobalConstants.year}`,
    );
  });

  it('los plazos salen de GlobalConstants', () => {
    const rule = rules.find((r) => r.title === 'Plazos');
    expect(rule).toBeDefined();
    expect(rule!.content).toContain(GlobalConstants.startRegistrationDate);
    expect(rule!.content).toContain(GlobalConstants.endRegistrationDate);
    expect(rule!.content).toContain(GlobalConstants.startReceptionDate);
    expect(rule!.content).toContain(GlobalConstants.endReceptionDate);
  });

  it('el número de edición sale de GlobalConstants', () => {
    const rule = rules.find((r) => r.title === 'Objeto del concurso');
    expect(rule).toBeDefined();
    expect(rule!.content).toContain(
      `${GlobalConstants.editionNumber} Concurso Homebrewer de Córdoba`,
    );
  });

  it('permite inscribir las 3 cervezas en la misma categoría', () => {
    const rule = rules.find((r) => r.title === 'Participación e inscripción');
    expect(rule!.content).toContain('pudiendo inscribir las 3 en la misma categoría');
    expect(rule!.content).not.toContain('no pertenezcan a la misma categoría');
  });

  it('los premios incluyen los premios especiales de esta edición', () => {
    const rule = rules.find((r) => r.title === 'Premios');
    expect(rule!.content).toContain('Rookie del Año');
    expect(rule!.content).toContain('Libros de Guarda');
  });
});
