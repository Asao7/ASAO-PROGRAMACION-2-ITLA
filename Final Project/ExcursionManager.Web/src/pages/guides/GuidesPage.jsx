import { useEffect, useState } from 'react';
import { getGuides, createGuide, updateGuide, deleteGuide } from '../../api/api';

export default function GuidesPage() {
  const [guides, setGuides] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState({
    fullName: '', idNumber: '', specialty: '', phone: '', email: ''
  });

  const fetchData = async () => {
    try {
      const res = await getGuides();
      setGuides(res.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  const openCreate = () => {
    setEditing(null);
    setForm({ fullName: '', idNumber: '', specialty: '', phone: '', email: '' });
    setShowModal(true);
  };

  const openEdit = (guide) => {
    setEditing(guide);
    setForm({ fullName: guide.fullName, idNumber: guide.idNumber,
               specialty: guide.specialty, phone: guide.phone, email: guide.email });
    setShowModal(true);
  };

  const handleSubmit = async () => {
    try {
      if (editing) await updateGuide(editing.id, form);
      else await createGuide(form);
      setShowModal(false);
      fetchData();
    } catch (err) {
      alert('Error saving guide');
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Delete this guide?')) return;
    await deleteGuide(id);
    fetchData();
  };

  if (loading) return <div className="loading">Loading guides...</div>;

  return (
    <div>
      <div className="page-header">
        <div>
          <h1>🧭 Guides</h1>
          <p>Manage all excursion guides</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}>+ New Guide</button>
      </div>

      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>Full Name</th>
              <th>ID Number</th>
              <th>Specialty</th>
              <th>Phone</th>
              <th>Email</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {guides.map(g => (
              <tr key={g.id}>
                <td><strong>{g.fullName}</strong></td>
                <td>{g.idNumber}</td>
                <td>{g.specialty}</td>
                <td>{g.phone}</td>
                <td>{g.email}</td>
                <td>
                  <span className={`badge ${g.isActive ? 'badge-green' : 'badge-red'}`}>
                    {g.isActive ? 'Active' : 'Inactive'}
                  </span>
                </td>
                <td style={{ display: 'flex', gap: 6 }}>
                  <button className="btn btn-warning btn-sm" onClick={() => openEdit(g)}>Edit</button>
                  <button className="btn btn-danger btn-sm" onClick={() => handleDelete(g.id)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>{editing ? 'Edit Guide' : 'New Guide'}</h2>
            {[['fullName','Full Name'],['idNumber','ID Number'],
              ['specialty','Specialty'],['phone','Phone'],['email','Email']
            ].map(([field, label]) => (
              <div className="form-group" key={field}>
                <label>{label}</label>
                <input value={form[field]}
                  onChange={e => setForm({...form, [field]: e.target.value})} />
              </div>
            ))}
            <div className="form-actions">
              <button className="btn btn-danger" onClick={() => setShowModal(false)}>Cancel</button>
              <button className="btn btn-primary" onClick={handleSubmit}>
                {editing ? 'Save Changes' : 'Create'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}