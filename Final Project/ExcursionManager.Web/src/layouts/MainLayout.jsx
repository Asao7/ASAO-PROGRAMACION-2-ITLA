import { NavLink } from 'react-router-dom';

export default function MainLayout({ children, user, onLogout }) {
  return (
    <div className="layout">
      <aside className="sidebar">
        <div className="sidebar-header">
          <h2>🏔️ Vaamonos</h2>
          <p>Guided Tours System</p>
        </div>
        <nav className="sidebar-nav">
          <div className="nav-section">Main</div>
          <NavLink to="/" className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}>
            <span className="nav-icon">📊</span> Dashboard
          </NavLink>

          <div className="nav-section">Management</div>
          <NavLink to="/excursions" className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}>
            <span className="nav-icon">🗺️</span> Excursions
          </NavLink>
          <NavLink to="/routes" className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}>
            <span className="nav-icon">🛤️</span> Routes
          </NavLink>
          <NavLink to="/guides" className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}>
            <span className="nav-icon">🧭</span> Guides
          </NavLink>
          <NavLink to="/participants" className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}>
            <span className="nav-icon">👥</span> Participants
          </NavLink>
          <NavLink to="/reservations" className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}>
            <span className="nav-icon">📋</span> Reservations
          </NavLink>
        </nav>

        <div style={{ position: 'absolute', bottom: 0, left: 0, right: 0, padding: '16px 20px', borderTop: '1px solid rgba(255,255,255,0.1)' }}>
          <div style={{ marginBottom: 10 }}>
            <p style={{ color: 'white', fontSize: 13, fontWeight: 600, margin: 0 }}>
              👤 {user?.fullName}
            </p>
            <p style={{ color: 'rgba(255,255,255,0.5)', fontSize: 11, margin: '2px 0 0' }}>
              {user?.role}
            </p>
          </div>
          <button onClick={onLogout} style={{
            width: '100%', padding: '8px',
            background: 'rgba(255,255,255,0.1)',
            color: 'white', border: '1px solid rgba(255,255,255,0.2)',
            borderRadius: 8, cursor: 'pointer',
            fontSize: 13, fontWeight: 500
          }}>
            🚪 Sign Out
          </button>
        </div>
      </aside>
      <main className="main-content">
        {children}
      </main>
    </div>
  );
}