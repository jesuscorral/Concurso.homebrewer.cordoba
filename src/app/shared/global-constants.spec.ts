import { describe, expect, it } from 'vitest';

import { GlobalConstants } from './global-constants';
import { parseSpanishDate } from './registration-window';

const MONTHS = [
  'enero', 'febrero', 'marzo', 'abril', 'mayo', 'junio',
  'julio', 'agosto', 'septiembre', 'octubre', 'noviembre', 'diciembre',
];

// Estas comprobaciones protegen la actualización anual de la edición:
// si al cambiar las fechas queda una incoherencia (rango invertido, mes con
// errata, evento anterior a la recepción...), el test falla en CI.
describe('GlobalConstants (fechas de la edición)', () => {
  it('la fecha del evento es válida', () => {
    expect(Number(GlobalConstants.day)).toBeGreaterThanOrEqual(1);
    expect(Number(GlobalConstants.day)).toBeLessThanOrEqual(31);
    expect(MONTHS).toContain(GlobalConstants.month.toLowerCase());
    expect(GlobalConstants.year).toMatch(/^\d{4}$/);
  });

  it('el plazo de inscripción está bien ordenado', () => {
    const start = parseSpanishDate(GlobalConstants.startRegistrationDate);
    const end = parseSpanishDate(GlobalConstants.endRegistrationDate);
    expect(start.getTime()).toBeLessThan(end.getTime());
  });

  it('si hay ampliación, el nuevo fin de inscripción es posterior al original', () => {
    if (!GlobalConstants.previousEndRegistrationDate) return;
    const previous = parseSpanishDate(GlobalConstants.previousEndRegistrationDate);
    const end = parseSpanishDate(GlobalConstants.endRegistrationDate);
    expect(previous.getTime()).toBeLessThan(end.getTime());
  });

  it('el plazo de recepción de botellas está bien ordenado', () => {
    const start = parseSpanishDate(GlobalConstants.startReceptionDate);
    const end = parseSpanishDate(GlobalConstants.endReceptionDate);
    expect(start.getTime()).toBeLessThan(end.getTime());
  });

  it('la inscripción termina como tarde el día que empieza la recepción', () => {
    const endRegistration = parseSpanishDate(GlobalConstants.endRegistrationDate);
    const startReception = parseSpanishDate(GlobalConstants.startReceptionDate);
    expect(endRegistration.getTime()).toBeLessThanOrEqual(startReception.getTime());
  });

  it('la recepción termina antes del día del concurso', () => {
    const endReception = parseSpanishDate(GlobalConstants.endReceptionDate);
    const eventDay = parseSpanishDate(`${GlobalConstants.day} de ${GlobalConstants.month}`);
    expect(endReception.getTime()).toBeLessThan(eventDay.getTime());
  });
});
