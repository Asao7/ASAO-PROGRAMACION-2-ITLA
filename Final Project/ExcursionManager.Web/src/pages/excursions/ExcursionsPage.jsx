import { useEffect, useState } from 'react';
import { getExcursions, createExcursion, updateExcursion, deleteExcursion, cancelExcursion, getGuides, getRoutes } from '../../api/api';

export default function ExcursionsPage() {
  const [excursions, setExcursions] = useState([]);
  const [guides, setGuides] = useState([]);
  const [routes, setRoutes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState({ name: '', description: '', routeId: '', guideId: '', departureDate: '', maxCapacity: '', price: '' });
  const fetchData = async () => {
  try {
    const [exc, gui, rou] = await Promise.all([
      getExcursions(), getGuides(), getRoutes()
    ]);
    setExcursions(exc.data);
    setGuides(gui.data);
    setRoutes(rou.data);
  } catch (err) {
    console.error(err);
  } finally {
    setLoading(false);
  }
};

  useEffect(() => { fetchData(); }, []);

  const openCreate = () => {
    setEditing(null);
    setForm({ name: '', description: '', routeId: '', guideId: '', departureDate: '', maxCapacity: '', price: '' });
    setShowModal(true);
  };

  const openEdit = (exc) => {
    setEditing(exc);
    setForm({ name: exc.name, description: exc.description, routeId: exc.routeId, guideId: exc.guideId || '', departureDate: exc.departureDate?.slice(0, 16), maxCapacity: exc.maxCapacity, price: exc.price });
    setShowModal(true);
  };

  const handleSubmit = async () => {
    try {
      const payload = { ...form, routeId: Number(form.routeId), guideId: form.guideId ? Number(form.guideId) : null, maxCapacity: Number(form.maxCapacity), price: Number(form.price) };
      if (editing) await updateExcursion(editing.id, payload);
      else await createExcursion(payload);
      setShowModal(false);
      fetchData();
    } catch (err) {
      alert(err.response?.data || 'Error saving excursion');
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Delete this excursion?')) return;
    await deleteExcursion(id);
    fetchData();
  };

  const handleCancel = async (id) => {
    if (!confirm('Cancel this excursion?')) return;
    await cancelExcursion(id);
    fetchData();
  };

  const statusBadge = (status) => {
    const map = { Active: 'badge-green', Full: 'badge-yellow', Cancelled: 'badge-red' };
    return <span className={`badge ${map[status] || 'badge-gray'}`}>{status}</span>;
  };

  if (loading) return <div className="loading">Loading excursions...</div>;

  return (
    <div>
      <div className="page-header">
        <div>
          <h1>Excursions</h1>
          <p>Manage all guided excursions</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}>+ New Excursion</button>
      </div>
      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>Name</th><th>Route</th><th>Guide</th><th>Departure</th><th>Spots</th><th>Price</th><th>Status</th><th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {excursions.map(exc => (
              <tr key={exc.id}>
                <td><strong>{exc.name}</strong></td>
                <td>{exc.routeName || 'Route #' + exc.routeId}</td>
                <td>{exc.guideName || 'Not assigned'}</td>
                <td>{new Date(exc.departureDate).toLocaleDateString()}</td>
                <td>{exc.availableSpots}/{exc.maxCapacity}</td>
                <td>${exc.price.toLocaleString()}</td>
                <td>{statusBadge(exc.status)}</td>
                <td style={{ display: 'flex', gap: 6 }}>
                  <button className="btn btn-warning btn-sm" onClick={() => openEdit(exc)}>Edit</button>
                  <button className="btn btn-danger btn-sm" onClick={() => handleCancel(exc.id)}>Cancel</button>
                  <button className="btn btn-danger btn-sm" onClick={() => handleDelete(exc.id)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>{editing ? 'Edit Excursion' : 'New Excursion'}</h2>
            <div className="form-group"><label>Name</label><input value={form.name} onChange={e => setForm({...form, name: e.target.value})} /></div>
            <div className="form-group"><label>Description</label><input value={form.description} onChange={e => setForm({...form, description: e.target.value})} /></div>
            <div className="form-group">
              <label>Route</label>
              <select value={form.routeId} onChange={e => setForm({...form, routeId: e.target.value})}>
                <option value="">-- Select Route --</option>
                {routes.map(r => <option key={r.id} value={r.id}>{r.name} ({r.difficulty})</option>)}
              </select>
            </div>
            <div className="form-group">
              <label>Guide</label>
              <select value={form.guideId} onChange={e => setForm({...form, guideId: e.target.value})}>
                <option value="">-- Select Guide --</option>
                {guides.map(g => <option key={g.id} value={g.id}>{g.fullName}</option>)}
              </select>
            </div>
            <div className="form-group"><label>Departure Date</label><input type="datetime-local" value={form.departureDate} onChange={e => setForm({...form, departureDate: e.target.value})} /></div>
            <div className="form-group"><label>Max Capacity</label><input type="number" value={form.maxCapacity} onChange={e => setForm({...form, maxCapacity: e.target.value})} /></div>
            <div className="form-group"><label>Price</label><input type="number" value={form.price} onChange={e => setForm({...form, price: e.target.value})} /></div>
            <div className="form-actions">
              <button className="btn btn-danger" onClick={() => setShowModal(false)}>Cancel</button>
              <button className="btn btn-primary" onClick={handleSubmit}>{editing ? 'Save Changes' : 'Create'}</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}