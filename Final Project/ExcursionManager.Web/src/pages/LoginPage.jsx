import { useState } from 'react';
import { login } from '../api/api';

export default function LoginPage({ onLogin }) {
  const [form, setForm] = useState({ username: '', password: '' });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async () => {
    if (!form.username || !form.password) {
      setError('Please enter your username and password.');
      return;
    }
    setLoading(true);
    setError('');
    try {
      const res = await login(form);
      localStorage.setItem('token', res.data.token);
      localStorage.setItem('user', JSON.stringify(res.data));
      onLogin(res.data);
    } catch (err) {
      setError('Invalid username or password.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{
      minHeight: '100vh',
      background: 'linear-gradient(135deg, #0D1F2D 0%, #1a3c5e 50%, #028090 100%)',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      position: 'relative',
      overflow: 'hidden'
    }}>
      {/* Background image */}
      <div style={{
        position: 'absolute', inset: 0,
        backgroundImage: 'url(https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=1200&q=80)',
        backgroundSize: 'cover',
        backgroundPosition: 'center',
        opacity: 0.2
      }} />

      {/* Login card */}
      <div style={{
        position: 'relative',
        background: 'white',
        borderRadius: 20,
        padding: '48px 40px',
        width: '100%',
        maxWidth: 420,
        boxShadow: '0 25px 60px rgba(0,0,0,0.4)'
      }}>
        {/* Logo */}
        <div style={{ textAlign: 'center', marginBottom: 32 }}>
          <div style={{ fontSize: 48, marginBottom: 8 }}>🏔️</div>
          <h1 style={{
            fontSize: 32, fontWeight: 700,
            color: '#1a3c5e', fontFamily: 'Georgia',
            margin: 0
          }}>Vaamonos</h1>
          <p style={{ color: '#888', fontSize: 14, margin: '6px 0 0' }}>
            Guided Tours System
          </p>
        </div>

        <h2 style={{ fontSize: 18, color: '#333', marginBottom: 24, fontWeight: 600 }}>
          Sign in to your account
        </h2>

        {error && (
          <div style={{
            background: '#ffebee', color: '#c62828',
            padding: '12px 16px', borderRadius: 8,
            marginBottom: 16, fontSize: 14
          }}>
            ⚠️ {error}
          </div>
        )}

        <div style={{ marginBottom: 16 }}>
          <label style={{ display: 'block', fontSize: 13, fontWeight: 500, color: '#555', marginBottom: 6 }}>
            Username
          </label>
          <input
            type="text"
            value={form.username}
            onChange={e => setForm({ ...form, username: e.target.value })}
            onKeyDown={e => e.key === 'Enter' && handleSubmit()}
            placeholder="Enter your username"
            style={{
              width: '100%', padding: '12px 16px',
              border: '1px solid #ddd', borderRadius: 10,
              fontSize: 15, outline: 'none',
              boxSizing: 'border-box'
            }}
          />
        </div>

        <div style={{ marginBottom: 24 }}>
          <label style={{ display: 'block', fontSize: 13, fontWeight: 500, color: '#555', marginBottom: 6 }}>
            Password
          </label>
          <input
            type="password"
            value={form.password}
            onChange={e => setForm({ ...form, password: e.target.value })}
            onKeyDown={e => e.key === 'Enter' && handleSubmit()}
            placeholder="Enter your password"
            style={{
              width: '100%', padding: '12px 16px',
              border: '1px solid #ddd', borderRadius: 10,
              fontSize: 15, outline: 'none',
              boxSizing: 'border-box'
            }}
          />
        </div>

        <button
          onClick={handleSubmit}
          disabled={loading}
          style={{
            width: '100%', padding: '14px',
            background: loading ? '#aaa' : 'linear-gradient(135deg, #1a3c5e, #028090)',
            color: 'white', border: 'none',
            borderRadius: 10, fontSize: 16,
            fontWeight: 600, cursor: loading ? 'not-allowed' : 'pointer',
            transition: 'all 0.2s'
          }}
        >
          {loading ? 'Signing in...' : 'Sign In'}
        </button>

        {/* Demo credentials */}
        <div style={{
          marginTop: 24, padding: 16,
          background: '#f0f4f8', borderRadius: 10
        }}>
          <p style={{ fontSize: 12, color: '#666', margin: '0 0 8px', fontWeight: 600 }}>
            Demo Credentials:
          </p>
          {[['admin', 'Admin — Full access'], ['carlos', 'Guide'], ['ana', 'Staff']].map(([u, label]) => (
            <div key={u}
              onClick={() => setForm({ username: u, password: 'Admin123!' })}
              style={{
                cursor: 'pointer', padding: '6px 10px',
                borderRadius: 6, marginBottom: 4,
                background: 'white', fontSize: 13, color: '#444',
                display: 'flex', justifyContent: 'space-between'
              }}
            >
              <span>👤 <strong>{u}</strong></span>
              <span style={{ color: '#888' }}>{label}</span>
            </div>
          ))}
          <p style={{ fontSize: 11, color: '#aaa', margin: '8px 0 0' }}>
            Password: Admin123! — Click to autofill
          </p>
        </div>
      </div>
    </div>
  );
}