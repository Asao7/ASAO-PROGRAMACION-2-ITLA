import { useEffect, useState } from 'react';
import { getRoutes, createRoute, updateRoute, deleteRoute } from '../../api/api';

export default function RoutesPage() {
  const [routes, setRoutes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState({
    name: '', description: '', distanceKm: '', difficulty: 'Easy', startPoint: '', endPoint: ''
  });

  const fetchData = async () => {
    try {
      const res = await getRoutes();
      setRoutes(res.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  const openCreate = () => {
    setEditing(null);
    setForm({ name: '', description: '', distanceKm: '', difficulty: 'Easy', startPoint: '', endPoint: '' });
    setShowModal(true);
  };

  const openEdit = (r) => {
    setEditing(r);
    setForm({ name: r.name, description: r.description, distanceKm: r.distanceKm, difficulty: r.difficulty, startPoint: r.startPoint, endPoint: r.endPoint });
    setShowModal(true);
  };

  const handleSubmit = async () => {
    try {
      const payload = { ...form, distanceKm: Number(form.distanceKm) };
      if (editing) await updateRoute(editing.id, payload);
      else await createRoute(payload);
      setShowModal(false);
      fetchData();
    } catch (err) {
      alert('Error saving route');
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Delete this route?')) return;
    await deleteRoute(id);
    fetchData();
  };

  const difficultyBadge = (d) => {
    const map = { Easy: 'badge-green', Moderate: 'badge-yellow', Hard: 'badge-red' };
    return <span className={`badge ${map[d] || 'badge-gray'}`}>{d}</span>;
  };

  if (loading) return <div className="loading">Loading routes...</div>;

  return (
    <div>
      <div className="page-header">
        <div>
          <h1>🗺️ Routes</h1>
          <p>Manage all excursion routes</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}>+ New Route</button>
      </div>

      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Difficulty</th>
              <th>Distance</th>
              <th>Start Point</th>
              <th>End Point</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {routes.map(r => (
              <tr key={r.id}>
                <td><strong>{r.name}</strong></td>
                <td>{difficultyBadge(r.difficulty)}</td>
                <td>{r.distanceKm} km</td>
                <td>{r.startPoint}</td>
                <td>{r.endPoint}</td>
                <td style={{ display: 'flex', gap: 6 }}>
                  <button className="btn btn-warning btn-sm" onClick={() => openEdit(r)}>Edit</button>
                  <button className="btn btn-danger btn-sm" onClick={() => handleDelete(r.id)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>{editing ? 'Edit Route' : 'New Route'}</h2>
            <div className="form-group"><label>Name</label><input value={form.name} onChange={e => setForm({...form, name: e.target.value})} /></div>
            <div className="form-group"><label>Description</label><input value={form.description} onChange={e => setForm({...form, description: e.target.value})} /></div>
            <div className="form-group">
              <label>Difficulty</label>
              <select value={form.difficulty} onChange={e => setForm({...form, difficulty: e.target.value})}>
                <option value="Easy">Easy</option>
                <option value="Moderate">Moderate</option>
                <option value="Hard">Hard</option>
              </select>
            </div>
            <div className="form-group"><label>Distance (km)</label><input type="number" value={form.distanceKm} onChange={e => setForm({...form, distanceKm: e.target.value})} /></div>
            <div className="form-group"><label>Start Point</label><input value={form.startPoint} onChange={e => setForm({...form, startPoint: e.target.value})} /></div>
            <div className="form-group"><label>End Point</label><input value={form.endPoint} onChange={e => setForm({...form, endPoint: e.target.value})} /></div>
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