export interface ParseResponse {
  costCentre: string;
  total: number;
  salesTax: number;
  totalExcludingTax: number;
  paymentMethod?: string;
  vendor?: string;
  description?: string;
  date?: string;
}