import { useState, useEffect } from 'react';
import { Container, Card, Alert } from 'react-bootstrap';
import ConversionForm from '../components/ConversionForm';
import ConversionResultDisplay from '../components/ConversionResultDisplay';
import ConversionHistoryTable from '../components/ConversionHistoryTable';
import {
  getCurrencies,
  createConversionResult,
  getConversionResults,
} from '../api/conversion';
import type { Currency, ConversionResult } from '../types';

const ConversionPage = () => {
  const [currencies, setCurrencies] = useState<Currency[]>([]);
  const [currencyFromId, setCurrencyFromId] = useState('');
  const [currencyToId, setCurrencyToId] = useState('');
  const [amount, setAmount] = useState('');
  const [lastResult, setLastResult] = useState<{
    amount: number;
    fromCode: string;
    toCode: string;
    result: number;
    rate: number;
  } | null>(null);
  const [history, setHistory] = useState<ConversionResult[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [loadingCurrencies, setLoadingCurrencies] = useState(true);
  const [loadingHistory, setLoadingHistory] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    getCurrencies()
      .then(setCurrencies)
      .catch((err) => setError(err?.response?.data?.message ?? 'Erreur chargement des devises'))
      .finally(() => setLoadingCurrencies(false));
  }, []);

  const loadHistory = () => {
    setLoadingHistory(true);
    getConversionResults()
      .then(setHistory)
      .catch(() => setHistory([]))
      .finally(() => setLoadingHistory(false));
  };

  useEffect(() => {
    loadHistory();
  }, []);

  const handleSubmit = async () => {
    if (!currencyFromId || !currencyToId || !amount) return;
    const numAmount = parseFloat(amount);
    if (Number.isNaN(numAmount) || numAmount <= 0) return;

    setError(null);
    setIsLoading(true);
    try {
      const fromId = Number(currencyFromId);
      const toId = Number(currencyToId);
      const created = await createConversionResult({
        currencyFromId: fromId,
        currencyToId: toId,
        value: numAmount,
      });
      setLastResult({
        amount: created.value,
        fromCode: created.currencyFromCode,
        toCode: created.currencyToCode,
        result: created.value * created.rate,
        rate: created.rate,
      });
      await loadHistory();
    } catch (err: unknown) {
      const message =
        (err as { response?: { data?: { message?: string } } })?.response?.data
          ?.message ?? 'Erreur lors de la conversion';
      setError(message);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Container className="py-4">
      {error && (
        <Alert variant="danger" dismissible onClose={() => setError(null)}>
          {error}
        </Alert>
      )}

      <Card className="mb-4">
        <Card.Header as="h4">Conversion de devises</Card.Header>
        <Card.Body>
          <ConversionForm
            currencies={currencies}
            currencyFromId={currencyFromId}
            currencyToId={currencyToId}
            amount={amount}
            onCurrencyFromChange={setCurrencyFromId}
            onCurrencyToChange={setCurrencyToId}
            onAmountChange={setAmount}
            onSubmit={handleSubmit}
            isLoading={isLoading}
          />
          {loadingCurrencies && (
            <p className="text-muted small mt-2">Chargement des devises...</p>
          )}
        </Card.Body>
      </Card>

      <ConversionResultDisplay
        amount={lastResult?.amount}
        fromCode={lastResult?.fromCode}
        toCode={lastResult?.toCode}
        result={lastResult?.result}
        rate={lastResult?.rate}
        showPlaceholder
      />

      <ConversionHistoryTable items={history} loading={loadingHistory} />
    </Container>
  );
};

export default ConversionPage;
