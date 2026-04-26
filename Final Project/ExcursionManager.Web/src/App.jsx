import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import MainLayout from './layouts/MainLayout';
import Dashboard from './pages/Dashboard';
import ExcursionsPage from './pages/excursions/ExcursionsPage';
import GuidesPage from './pages/guides/GuidesPage';
import ParticipantsPage from './pages/participants/ParticipantsPage';
import ReservationsPage from './pages/reservations/ReservationsPage';
import LoginPage from './pages/LoginPage';
import RoutesPage from './pages/routes/RoutesPage';

export default function App() {
  const [user, setUser] = useState(null);

  useEffect(() => {
    const stored = localStorage.getItem('user');
    if (stored) setUser(JSON.parse(stored));
  }, []);

  const handleLogin = (userData) => {
    setUser(userData);
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setUser(null);
  };

  if (!user) return <LoginPage onLogin={handleLogin} />;

  return (
    <BrowserRouter>
      <MainLayout user={user} onLogout={handleLogout}>
        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/excursions" element={<ExcursionsPage />} />
          <Route path="/guides" element={<GuidesPage />} />
          <Route path="/participants" element={<ParticipantsPage />} />
          <Route path="/reservations" element={<ReservationsPage />} />
          <Route path="/routes" element={<RoutesPage />} />
          <Route path="*" element={<Navigate to="/" />} />
        </Routes>
      </MainLayout>
    </BrowserRouter>
  );
}