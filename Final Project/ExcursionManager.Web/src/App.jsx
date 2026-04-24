import { BrowserRouter, Routes, Route } from 'react-router-dom';
import MainLayout from './layouts/MainLayout';
import Dashboard from './pages/Dashboard';
import ExcursionsPage from './pages/excursions/ExcursionsPage';
import GuidesPage from './pages/guides/GuidesPage';
import ParticipantsPage from './pages/participants/ParticipantsPage';
import ReservationsPage from './pages/reservations/ReservationsPage';

export default function App() {
  return (
    <BrowserRouter>
      <MainLayout>
        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/excursions" element={<ExcursionsPage />} />
          <Route path="/guides" element={<GuidesPage />} />
          <Route path="/participants" element={<ParticipantsPage />} />
          <Route path="/reservations" element={<ReservationsPage />} />
        </Routes>
      </MainLayout>
    </BrowserRouter>
  );
}