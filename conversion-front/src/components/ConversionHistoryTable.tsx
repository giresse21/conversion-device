import type { ConversionResult } from '../types';
import { Table, Card } from 'react-bootstrap';

export interface ConversionHistoryTableProps {
  items: ConversionResult[];
  loading?: boolean;
}

const formatDate = (isoDate: string) => {
  const d = new Date(isoDate);
  return d.toLocaleString(undefined, {
    dateStyle: 'short',
    timeStyle: 'short',
  });
};

const ConversionHistoryTable = ({
  items,
  loading = false,
}: ConversionHistoryTableProps) => {
  return (
    <Card className="mt-4">
      <Card.Header as="h5">Historique des conversions</Card.Header>
      <Card.Body>
        {loading ? (
          <p className="text-muted mb-0">Chargement...</p>
        ) : items.length === 0 ? (
          <p className="text-muted mb-0">Aucune conversion enregistrée.</p>
        ) : (
          <Table responsive striped bordered hover>
            <thead>
              <tr>
                <th>Devise départ</th>
                <th>Devise arrivée</th>
                <th>Montant</th>
                <th>Taux</th>
                <th>Résultat</th>
                <th>Date</th>
              </tr>
            </thead>
            <tbody>
              {items.map((row) => (
                <tr key={row.id}>
                  <td>{row.currencyFromCode}</td>
                  <td>{row.currencyToCode}</td>
                  <td>{row.value}</td>
                  <td>{row.rate}</td>
                  <td>{(row.value * row.rate).toFixed(2)}</td>
                  <td>{formatDate(row.createdAt)}</td>
                </tr>
              ))}
            </tbody>
          </Table>
        )}
      </Card.Body>
    </Card>
  );
};

export default ConversionHistoryTable;
