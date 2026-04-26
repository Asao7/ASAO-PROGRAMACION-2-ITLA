import { useEffect, useState } from 'react';
import {
  getReservations, createReservation,
  confirmReservation, cancelReservation, markAttendance
} from '../../api/api';
import { getExcursions } from '../../api/api';
import { getParticipants } from '../../api/api';

export default function ReservationsPage() {
  const [reservations, setReservations] = useState([]);
  const [excursions, setExcursions] = useState([]);
  const [participants, setParticipants] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [form, setForm] = useState({
  participantId: '',
  excursionId: '',
  status: '',
  attended: false
});
  const [editing, setEditing] = useState(null);

  const fetchData = async () => {
    try {
      const [res, exc, par] = await Promise.all([
        getReservations(), getExcursions(), getParticipants()
      ]);
      setReservations(res.data);
      setExcursions(exc.data.filter(e => e.status === 'Active'));
      setParticipants(par.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  const handleSubmit = async () => {
    try {
      await createReservation({
        participantId: Number(form.participantId),
        excursionId: Number(form.excursionId)
      });
      setShowModal(false);
      fetchData();
    } catch (err) {
      alert(err.response?.data || 'Error creating reservation');
    }
  };

  const statusBadge = (status) => {
    const map = { Pending: 'badge-yellow', Confirmed: 'badge-green', Cancelled: 'badge-red' };
    return <span className={`badge ${map[status] || 'badge-gray'}`}>{status}</span>;
  };

  if (loading) return <div className="loading">Loading reservations...</div>;

  return (
    <div>
      <div className="page-header">
        <div>
          <h1>📋 Reservations</h1>
          <p>Manage all excursion reservations</p>
        </div>
        <button className="btn btn-primary" onClick={() => {
          setForm({ participantId: '', excursionId: '' });
          setShowModal(true);
        }}>+ New Reservation</button>
      </div>

      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>Participant</th>
              <th>Excursion</th>
              <th>Reserved At</th>
              <th>Status</th>
              <th>Attended</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {reservations.map(r => (
              <tr key={r.id}>
                <td><strong>{r.participantName}</strong></td>
                <td>{r.excursionName}</td>
                <td>{new Date(r.reservedAt).toLocaleDateString()}</td>
                <td>{statusBadge(r.status)}</td>
                <td>
                  <span className={`badge ${r.attended ? 'badge-green' : 'badge-gray'}`}>
                    {r.attended ? 'Yes' : 'No'}
                  </span>
                </td>
                <td style={{ display: 'flex', gap: 6, flexWrap: 'wrap' }}>
                  {r.status === 'Pending' && (
                    <button className="btn btn-success btn-sm"
                      onClick={async () => { await confirmReservation(r.id); fetchData(); }}>
                      Confirm
                    </button>
                  )}
                  {r.status === 'Confirmed' && !r.attended && (
                    <button className="btn btn-primary btn-sm"
                      onClick={async () => { await markAttendance(r.id); fetchData(); }}>
                      ✓ Attended
                    </button>
                  )}
                  {r.status !== 'Cancelled' && (
                    <button className="btn btn-danger btn-sm"
                      onClick={async () => {
                        if (!confirm('Cancel this reservation?')) return;
                        await cancelReservation(r.id); fetchData();
                      }}>
                      Cancel
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>New Reservation</h2>
            <div className="form-group">
              <label>Participant</label>
              <select value={form.participantId}
                onChange={e => setForm({...form, participantId: e.target.value})}>
                <option value="">-- Select Participant --</option>
                {participants.map(p => (
                  <option key={p.id} value={p.id}>{p.fullName}</option>
                ))}
              </select>
            </div>
            <div className="form-group">
              <label>Excursion</label>
              <select value={form.excursionId}
                onChange={e => setForm({...form, excursionId: e.target.value})}>
                <option value="">-- Select Excursion --</option>
                {excursions.map(e => (
                  <option key={e.id} value={e.id}>
                    {e.name} ({e.availableSpots} spots left)
                  </option>
                ))}
              </select>
            </div>
            <div className="form-actions">
              <button className="btn btn-danger" onClick={() => setShowModal(false)}>Cancel</button>
              <button className="btn btn-primary" onClick={handleSubmit}>Create Reservation</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}