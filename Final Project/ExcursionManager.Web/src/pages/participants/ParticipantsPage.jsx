import { useEffect, useState } from 'react';
import { getParticipants, createParticipant, updateParticipant, deleteParticipant } from '../../api/api';

export default function ParticipantsPage() {
  const [participants, setParticipants] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState({
    fullName: '', idNumber: '', age: '', emergencyContact: '', email: ''
  });

  const fetchData = async () => {
    try {
      const res = await getParticipants();
      setParticipants(res.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  const openCreate = () => {
    setEditing(null);
    setForm({ fullName: '', idNumber: '', age: '', emergencyContact: '', email: '' });
    setShowModal(true);
  };

  const openEdit = (p) => {
    setEditing(p);
    setForm({ fullName: p.fullName, idNumber: p.idNumber, age: p.age,
               emergencyContact: p.emergencyContact, email: p.email });
    setShowModal(true);
  };

  const handleSubmit = async () => {
    try {
      const payload = { ...form, age: Number(form.age) };
      if (editing) await updateParticipant(editing.id, payload);
      else await createParticipant(payload);
      setShowModal(false);
      fetchData();
    } catch (err) {
      alert(err.response?.data || 'Error saving participant');
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Delete this participant?')) return;
    await deleteParticipant(id);
    fetchData();
  };

  if (loading) return <div className="loading">Loading participants...</div>;

  return (
    <div>
      <div className="page-header">
        <div>
          <h1>👥 Participants</h1>
          <p>Manage all registered participants</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}>+ New Participant</button>
      </div>

      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>Full Name</th>
              <th>ID Number</th>
              <th>Age</th>
              <th>Emergency Contact</th>
              <th>Email</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {participants.map(p => (
              <tr key={p.id}>
                <td><strong>{p.fullName}</strong></td>
                <td>{p.idNumber}</td>
                <td>{p.age}</td>
                <td>{p.emergencyContact}</td>
                <td>{p.email}</td>
                <td style={{ display: 'flex', gap: 6 }}>
                  <button className="btn btn-warning btn-sm" onClick={() => openEdit(p)}>Edit</button>
                  <button className="btn btn-danger btn-sm" onClick={() => handleDelete(p.id)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>{editing ? 'Edit Participant' : 'New Participant'}</h2>
            {[['fullName','Full Name'],['idNumber','ID Number'],
              ['emergencyContact','Emergency Contact'],['email','Email']
            ].map(([field, label]) => (
              <div className="form-group" key={field}>
                <label>{label}</label>
                <input value={form[field]}
                  onChange={e => setForm({...form, [field]: e.target.value})} />
              </div>
            ))}
            <div className="form-group">
              <label>Age</label>
              <input type="number" value={form.age}
                onChange={e => setForm({...form, age: e.target.value})} />
            </div>
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