import { describe, expect, it } from 'vitest';

import { GlobalConstants } from './global-constants';
import { parseSpanishDate, registrationStatus } from './registration-window';

describe('registrationStatus', () => {
  const start = parseSpanishDate(GlobalConstants.startRegistrationDate);
  const end = parseSpanishDate(GlobalConstants.endRegistrationDate);

  it('antes del inicio, la inscripción no está abierta', () => {
    expect(registrationStatus(new Date(start.getTime() - 1))).toBe('not-open');
  });

  it('el primer día del plazo está abierta', () => {
    expect(registrationStatus(start)).toBe('open');
  });

  it('el último día del plazo cuenta entero', () => {
    const lastMinute = new Date(end);
    lastMinute.setHours(23, 59, 59);
    expect(registrationStatus(lastMinute)).toBe('open');
  });

  it('al día siguiente del fin, está cerrada', () => {
    const nextDay = new Date(end);
    nextDay.setDate(nextDay.getDate() + 1);
    expect(registrationStatus(nextDay)).toBe('closed');
  });

  it('rechaza fechas mal escritas', () => {
    expect(() => parseSpanishDate('7 Noviembre')).toThrow();
    expect(() => parseSpanishDate('7 de Noviembr')).toThrow();
  });
});
