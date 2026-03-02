import type { Currency } from '../types';
import { Form, Button, Row, Col } from 'react-bootstrap';

export interface ConversionFormProps {
  currencies: Currency[];
  currencyFromId: string;
  currencyToId: string;
  amount: string;
  onCurrencyFromChange: (value: string) => void;
  onCurrencyToChange: (value: string) => void;
  onAmountChange: (value: string) => void;
  onSubmit: () => void;
  isLoading?: boolean;
}

const ConversionForm = ({
  currencies,
  currencyFromId,
  currencyToId,
  amount,
  onCurrencyFromChange,
  onCurrencyToChange,
  onAmountChange,
  onSubmit,
  isLoading = false,
}: ConversionFormProps) => {
  return (
    <Form
      onSubmit={(e) => {
        e.preventDefault();
        onSubmit();
      }}
    >
      <Row className="g-3 align-items-end">
        <Col md={3}>
          <Form.Group controlId="currencyFrom">
            <Form.Label>Devise de départ</Form.Label>
            <Form.Select
              value={currencyFromId}
              onChange={(e) => onCurrencyFromChange(e.target.value)}
              aria-label="Choisir la devise de départ"
            >
              <option value="">-- Choisir --</option>
              {currencies.map((c) => (
                <option key={c.id} value={c.id}>
                  {c.code} - {c.name}
                </option>
              ))}
            </Form.Select>
          </Form.Group>
        </Col>
        <Col md={3}>
          <Form.Group controlId="currencyTo">
            <Form.Label>Devise d&apos;arrivée</Form.Label>
            <Form.Select
              value={currencyToId}
              onChange={(e) => onCurrencyToChange(e.target.value)}
              aria-label="Choisir la devise d'arrivée"
            >
              <option value="">-- Choisir --</option>
              {currencies.map((c) => (
                <option key={c.id} value={c.id}>
                  {c.code} - {c.name}
                </option>
              ))}
            </Form.Select>
          </Form.Group>
        </Col>
        <Col md={3}>
          <Form.Group controlId="amount">
            <Form.Label>Montant</Form.Label>
            <Form.Control
              type="number"
              min="0"
              step="any"
              placeholder="0.00"
              value={amount}
              onChange={(e) => onAmountChange(e.target.value)}
              aria-label="Montant à convertir"
            />
          </Form.Group>
        </Col>
        <Col md={3}>
          <Button type="submit" variant="primary" disabled={isLoading}>
            {isLoading ? 'Conversion...' : 'Convertir'}
          </Button>
        </Col>
      </Row>
    </Form>
  );
};

export default ConversionForm;
