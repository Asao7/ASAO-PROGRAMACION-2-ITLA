import { useEffect, useState } from 'react';
import { getExcursions, getGuides, getParticipants, getReservations } from '../api/api';

const GALLERY = [
  { url: 'https://realestatelasterrenas.com/wp-content/uploads/2025/04/Screenshot_1-723x467.png', caption: 'Pico Duarte' },
  { url: 'https://dynamic-media.tacdn.com/media/photo-o/30/8c/8e/a7/caption.jpg?w=1400&h=1000&s=1', caption: 'ATV y Buggy 4x4 en Punta Cana ' },
  { url: 'https://media.tacdn.com/media/attractions-splice-spp-674x446/0b/ba/44/94.jpg', caption: 'Cave Excursions' },
  { url: 'https://dynamic-media-cdn.tripadvisor.com/media/photo-o/32/97/be/a0/caption.jpg?w=1200&h=-1&s=1', caption: 'Cibao Valley' },
  { url: 'https://resources.diariolibre.com/images/2024/03/21/valle-nuevo-y-la-piramide-deleite-para-los-sentidos-focus-0-0-1228-688.jpg', caption: 'Valle Nuevo Constanza' },
  { url: 'https://images.unsplash.com/photo-1483728642387-6c3bdd6c93e5?w=400&q=80', caption: 'and More' },
];

export default function Dashboard() {
  const [stats, setStats] = useState({ excursions: 0, guides: 0, participants: 0, reservations: 0 });

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

      {/* Stats */}
      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-icon" style={{ background: '#e3f2fd' }}>🗺️</div>
          <div className="stat-info"><h3>{stats.excursions}</h3><p>Total Excursions</p></div>
        </div>
        <div className="stat-card">
          <div className="stat-icon" style={{ background: '#e8f5e9' }}>🧭</div>
          <div className="stat-info"><h3>{stats.guides}</h3><p>Total Guides</p></div>
        </div>
        <div className="stat-card">
          <div className="stat-icon" style={{ background: '#fff8e1' }}>👥</div>
          <div className="stat-info"><h3>{stats.participants}</h3><p>Total Participants</p></div>
        </div>
        <div className="stat-card">
          <div className="stat-icon" style={{ background: '#fce4ec' }}>📋</div>
          <div className="stat-info"><h3>{stats.reservations}</h3><p>Total Reservations</p></div>
        </div>
      </div>

      {/* Gallery */}
      <div style={{ marginTop: 32 }}>
        <h2 style={{ fontSize: 20, fontWeight: 700, color: '#1a3c5e', marginBottom: 16 }}>
          🏔️ Our Excursions
        </h2>
        <p style={{ color: '#888', marginBottom: 20, fontSize: 14 }}>
          A glimpse of the adventures waiting for you
        </p>
        <div style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))',
          gap: 16
        }}>
          {GALLERY.map((img, i) => (
            <div key={i} style={{
              borderRadius: 12,
              overflow: 'hidden',
              boxShadow: '0 4px 12px rgba(0,0,0,0.1)',
              position: 'relative',
              height: 200,
              cursor: 'pointer',
              transition: 'transform 0.2s',
            }}
              onMouseEnter={e => e.currentTarget.style.transform = 'scale(1.02)'}
              onMouseLeave={e => e.currentTarget.style.transform = 'scale(1)'}
            >
              <img src={img.url} alt={img.caption} style={{
                width: '100%', height: '100%', objectFit: 'cover'
              }} />
              <div style={{
                position: 'absolute', bottom: 0, left: 0, right: 0,
                background: 'linear-gradient(transparent, rgba(0,0,0,0.7))',
                padding: '24px 16px 12px',
              }}>
                <p style={{ color: 'white', fontWeight: 600, fontSize: 15 }}>{img.caption}</p>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}