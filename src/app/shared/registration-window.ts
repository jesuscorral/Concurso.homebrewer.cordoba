import { GlobalConstants } from './global-constants';

const MONTHS = [
  'enero', 'febrero', 'marzo', 'abril', 'mayo', 'junio',
  'julio', 'agosto', 'septiembre', 'octubre', 'noviembre', 'diciembre',
];

export type RegistrationStatus = 'not-open' | 'open' | 'closed';

/** Convierte "9 de Noviembre" en una fecha (00:00 local) del año de la edición. */
export function parseSpanishDate(value: string, year = Number(GlobalConstants.year)): Date {
  const match = /^(\d{1,2}) de ([a-záéíóúñ]+)$/i.exec(value.trim());
  const monthIndex = match ? MONTHS.indexOf(match[2].toLowerCase()) : -1;
  if (!match || monthIndex < 0) {
    throw new Error(`Fecha "${value}" no tiene el formato "<día> de <Mes>"`);
  }
  return new Date(year, monthIndex, Number(match[1]));
}

/**
 * Estado del plazo de inscripción en el instante `now`.
 * El último día del plazo cuenta entero (hasta las 23:59:59).
 */
export function registrationStatus(now: Date = new Date()): RegistrationStatus {
  const start = parseSpanishDate(GlobalConstants.startRegistrationDate);
  const end = parseSpanishDate(GlobalConstants.endRegistrationDate);
  end.setDate(end.getDate() + 1);
  if (now < start) return 'not-open';
  if (now >= end) return 'closed';
  return 'open';
}
