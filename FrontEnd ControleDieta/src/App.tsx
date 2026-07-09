import { useState, useEffect } from 'react';
import { authService } from './services/api';
import AuthView from './components/AuthView';
import ProfileSetup from './components/ProfileSetup';
import Dashboard from './components/Dashboard';
import './index.css';

// Views: 'auth' | 'profile' | 'dashboard'
type View = 'auth' | 'profile' | 'dashboard';

interface AppState {
  userId: number;
  userName: string;
  metaDiaria: number;
}

export default function App() {
  const [view, setView] = useState<View>('auth');
  const [appState, setAppState] = useState<AppState>({ userId: 0, userName: '', metaDiaria: 0 });

  useEffect(() => {
    // On load, check if user is already logged in
    if (authService.isLoggedIn()) {
      const user = authService.getUser();
      if (user) {
        setAppState(prev => ({
          ...prev,
          userId: user.userId,
          userName: user.userName,
        }));

        // Check localStorage for cached metaDiaria
        const cachedMeta = localStorage.getItem('diet_meta_diaria');
        const cachedName = localStorage.getItem('diet_user_name');
        if (cachedMeta && Number(cachedMeta) > 0) {
          setAppState({
            userId: user.userId,
            userName: cachedName || user.userName,
            metaDiaria: Number(cachedMeta),
          });
          setView('dashboard');
        } else {
          setView('profile');
        }
      }
    }
  }, []);

  const handleLoginSuccess = () => {
    const user = authService.getUser();
    if (user) {
      setAppState(prev => ({ ...prev, userId: user.userId, userName: user.userName }));
      // Check if profile already set up
      const cachedMeta = localStorage.getItem('diet_meta_diaria');
      if (cachedMeta && Number(cachedMeta) > 0) {
        const cachedName = localStorage.getItem('diet_user_name');
        setAppState({
          userId: user.userId,
          userName: cachedName || user.userName,
          metaDiaria: Number(cachedMeta),
        });
        setView('dashboard');
      } else {
        setView('profile');
      }
    }
  };

  const handleProfileComplete = (metaDiaria: number, name: string) => {
    localStorage.setItem('diet_meta_diaria', String(metaDiaria));
    localStorage.setItem('diet_user_name', name);
    setAppState(prev => ({ ...prev, metaDiaria, userName: name }));
    setView('dashboard');
  };

  const handleLogout = () => {
    authService.logout();
    localStorage.removeItem('diet_meta_diaria');
    localStorage.removeItem('diet_user_name');
    setAppState({ userId: 0, userName: '', metaDiaria: 0 });
    setView('auth');
  };

  return (
    <>
      {view === 'auth' && (
        <AuthView onLoginSuccess={handleLoginSuccess} />
      )}
      {view === 'profile' && (
        <ProfileSetup onProfileComplete={handleProfileComplete} />
      )}
      {view === 'dashboard' && (
        <Dashboard
          userId={appState.userId}
          userName={appState.userName}
          metaDiaria={appState.metaDiaria}
          onLogout={handleLogout}
        />
      )}
    </>
  );
}
