import axios from 'axios';

const API_URL = 'https://localhost:7056/api';

const api = axios.create({
  baseURL: API_URL,
  headers: { 'Content-Type': 'application/json' }
});

// ─── Excursions ───────────────────────────
export const getExcursions = () => api.get('/excursions');
export const getAvailableExcursions = () => api.get('/excursions/available');
export const getExcursionById = (id) => api.get(`/excursions/${id}`);
export const createExcursion = (data) => api.post('/excursions', data);
export const updateExcursion = (id, data) => api.put(`/excursions/${id}`, data);
export const cancelExcursion = (id) => api.patch(`/excursions/${id}/cancel`);
export const deleteExcursion = (id) => api.delete(`/excursions/${id}`);

// ─── Guides ───────────────────────────────
export const getGuides = () => api.get('/guides');
export const getActiveGuides = () => api.get('/guides/active');
export const getGuideById = (id) => api.get(`/guides/${id}`);
export const searchGuides = (name) => api.get(`/guides/search?name=${name}`);
export const createGuide = (data) => api.post('/guides', data);
export const updateGuide = (id, data) => api.put(`/guides/${id}`, data);
export const deleteGuide = (id) => api.delete(`/guides/${id}`);

// ─── Participants ─────────────────────────
export const getParticipants = () => api.get('/participants');
export const getParticipantById = (id) => api.get(`/participants/${id}`);
export const createParticipant = (data) => api.post('/participants', data);
export const updateParticipant = (id, data) => api.put(`/participants/${id}`, data);
export const deleteParticipant = (id) => api.delete(`/participants/${id}`);

// ─── Reservations ─────────────────────────
export const getReservations = () => api.get('/reservations');
export const getReservationById = (id) => api.get(`/reservations/${id}`);
export const getReservationsByExcursion = (id) => api.get(`/reservations/excursion/${id}`);
export const createReservation = (data) => api.post('/reservations', data);
export const confirmReservation = (id) => api.patch(`/reservations/${id}/confirm`);
export const cancelReservation = (id) => api.patch(`/reservations/${id}/cancel`);
export const markAttendance = (id) => api.patch(`/reservations/${id}/attendance`);
export const updateReservation = (id, data) => api.put(`/reservations/${id}`, data);
export const deleteReservation = (id) => api.delete(`/reservations/${id}`);

// ─── Auth ─────────────────────────────────
export const login = (data) => api.post('/auth/login', data);
export const register = (data) => api.post('/auth/register', data);

// ─── Axios interceptor - add token to every request ───
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});
// ─── Routes ───────────────────────────────
export const getRoutes = () => api.get('/routes');
export const createRoute = (data) => api.post('/routes', data);
export const updateRoute = (id, data) => api.put(`/routes/${id}`, data);
export const deleteRoute = (id) => api.delete(`/routes/${id}`);
