import { useEffect, useState } from 'react';
import { getExcursions } from '../api/api';
import { getGuides } from '../api/api';
import { getParticipants } from '../api/api';
import { getReservations } from '../api/api';

export default function Dashboard() {
  const [stats, setStats] = useState({
    excursions: 0, guides: 0, participants: 0, reservations: 0
  });

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const [exc, gui, par, res] = await Promise.all([
          getExcursions(), getGuides(), getParticipants(), getReservations()
        ]);
        setStats({
          excursions: exc.data.length,
          guides: gui.data.length,
          participants: par.data.length,
          reservations: res.data.length
        });
      } catch (err) {
        console.error('Error loading stats:', err);
      }
    };
    fetchStats();
  }, []);

  return (
    <div>
      <div className="page-header">
        <div>
          <h1>Dashboard</h1>
          <p>Welcome to the Excursion Manager System</p>
        </div>
      </div>

      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-icon" style={{ background: '#e3f2fd' }}>🗺️</div>
          <div className="stat-info">
            <h3>{stats.excursions}</h3>
            <p>Total Excursions</p>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon" style={{ background: '#e8f5e9' }}>🧭</div>
          <div className="stat-info">
            <h3>{stats.guides}</h3>
            <p>Total Guides</p>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon" style={{ background: '#fff8e1' }}>👥</div>
          <div className="stat-info">
            <h3>{stats.participants}</h3>
            <p>Total Participants</p>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon" style={{ background: '#fce4ec' }}>📋</div>
          <div className="stat-info">
            <h3>{stats.reservations}</h3>
            <p>Total Reservations</p>
          </div>
        </div>
      </div>
    </div>
  );
}