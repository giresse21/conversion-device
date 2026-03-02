export interface ConvertParams {
  from: string;
  to: string;
  amount: number;
}

export interface ConvertResponse {
  amount: number;
  from: string;
  to: string;
}

export interface Currency {
  id: number;
  name: string;
  code: string;
}

export interface ConversionResult {
  id: number;
  currencyFromCode: string;
  currencyToCode: string;
  rate: number;
  value: number;
  createdAt: string;
}

export interface CurrencyRate {
  id: number;
  currencyFromCode: string;
  currencyToCode: string;
  rate: number;
}