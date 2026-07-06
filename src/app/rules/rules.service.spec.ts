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
});
