import { Card } from 'react-bootstrap';

export interface ConversionResultDisplayProps {
  /** Montant saisi (devise de départ) */
  amount?: number;
  /** Code devise de départ (ex: EUR) */
  fromCode?: string;
  /** Code devise d'arrivée (ex: USD) */
  toCode?: string;
  /** Montant converti (résultat) */
  result?: number;
  /** Taux utilisé pour la conversion */
  rate?: number;
  /** Afficher la zone même sans résultat (message d'attente) */
  showPlaceholder?: boolean;
}

const ConversionResultDisplay = ({
  amount,
  fromCode,
  toCode,
  result,
  rate,
  showPlaceholder = true,
}: ConversionResultDisplayProps) => {
  const hasResult =
    amount != null && fromCode && toCode && result != null && rate != null;

  if (!hasResult && !showPlaceholder) return null;

  if (!hasResult) {
    return (
      <Card className="mt-4">
        <Card.Body>
          <Card.Text className="text-muted mb-0">
            Renseignez les devises et le montant puis cliquez sur Convertir pour
            afficher le résultat.
          </Card.Text>
        </Card.Body>
      </Card>
    );
  }

  return (
    <Card className="mt-4">
      <Card.Header as="h5">Résultat de la conversion</Card.Header>
      <Card.Body>
        <p className="mb-1">
          <strong>
            {amount} {fromCode}
          </strong>
          {' = '}
          <strong className="text-primary">
            {result.toFixed(2)} {toCode}
          </strong>
        </p>
        <p className="text-muted small mb-0">
          Taux utilisé : 1 {fromCode} = {rate} {toCode}
        </p>
      </Card.Body>
    </Card>
  );
};

export default ConversionResultDisplay;
