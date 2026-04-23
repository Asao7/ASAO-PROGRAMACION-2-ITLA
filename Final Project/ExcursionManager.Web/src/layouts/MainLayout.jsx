import { NavLink } from 'react-router-dom';

export default function MainLayout({ children }) {
  return (
    <div className="layout">
      <aside className="sidebar">
        <div className="sidebar-header">
          <h2>🏔️ Vaamonos!</h2>
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
      </aside>
      <main className="main-content">
        {children}
      </main>
    </div>
  );
}