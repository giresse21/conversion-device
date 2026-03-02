import { BrowserRouter, Routes, Route } from 'react-router-dom';
import ConversionPage from './pages/ConversionPage';
import './App.css';

const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<ConversionPage />} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;
