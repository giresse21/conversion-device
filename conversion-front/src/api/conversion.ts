import { apiClient } from './client';
import type { Currency, ConversionResult, CurrencyRate } from '../types';

export async function getCurrencies(): Promise<Currency[]> {
  const { data } = await apiClient.get<Currency[]>('/api/currencies');
  return data;
}

export async function getRate(
  currencyFromId: number,
  currencyToId: number
): Promise<CurrencyRate | null> {
  const { data } = await apiClient.get<CurrencyRate>(
    `/api/currencyrates/from/${currencyFromId}/to/${currencyToId}`
  );
  return data;
}

export interface CreateConversionResultPayload {
  currencyFromId: number;
  currencyToId: number;
  value: number;
}

export async function createConversionResult(
  payload: CreateConversionResultPayload
): Promise<ConversionResult> {
  const { data } = await apiClient.post<ConversionResult>(
    '/api/conversionresults',
    payload
  );
  return data;
}

export async function getConversionResults(): Promise<ConversionResult[]> {
  const { data } = await apiClient.get<ConversionResult[]>(
    '/api/conversionresults'
  );
  return data;
}
