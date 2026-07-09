import React, { useState } from 'react';
import { LogIn, UserPlus, Mail, Lock, User, AlertCircle, CheckCircle2 } from 'lucide-react';
import { apiFetch, authService } from '../services/api';

interface AuthViewProps {
  onLoginSuccess: () => void;
}

export default function AuthView({ onLoginSuccess }: AuthViewProps) {
  const [isLogin, setIsLogin] = useState(true);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [userName, setUserName] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');
    setLoading(true);

    try {
      if (isLogin) {
        // Log in
        // Note: UserName is required by LoginDto validation in .NET even if we log in via email.
        const response = await apiFetch<{ mensagem: string; token: { token: string } }>('/user/login', {
          method: 'POST',
          body: JSON.stringify({
            email,
            password,
            userName: email // Pass email as username to satisfy backend validation
          })
        });

        // Store JWT token. The backend returns nested Token: { token: "..." } in UserTokenDto format.
        // Let's check both possibilities (response.token.token or response.token)
        let tokenStr = '';
        if (response && response.token) {
          if (typeof response.token === 'object' && 'token' in response.token) {
            tokenStr = (response.token as any).token;
          } else if (typeof response.token === 'string') {
            tokenStr = response.token;
          }
        }

        if (tokenStr) {
          authService.setToken(tokenStr);
          onLoginSuccess();
        } else {
          throw new Error('Formato de resposta inválido do servidor (token não encontrado).');
        }
      } else {
        // Register
        await apiFetch<{ mensagem: string }>('/user/Register', {
          method: 'POST',
          body: JSON.stringify({
            email,
            password,
            userName
          })
        });

        setSuccess('Cadastro realizado com sucesso! Faça login para continuar.');
        setIsLogin(true);
        // Clear fields
        setPassword('');
      }
    } catch (err: any) {
      console.error(err);
      setError(err.message || 'Ocorreu um erro. Tente novamente mais tarde.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex-1 flex items-center justify-center p-6 min-h-[80vh]">
      <div className="w-full max-w-md glass-panel relative overflow-hidden">
        {/* Visual accents */}
        <div className="absolute top-0 left-0 right-0 h-[4px] bg-gradient-to-r from-emerald-500 to-indigo-500" style={{ background: 'linear-gradient(90deg, #10b981 0%, #6366f1 100%)' }}></div>
        
        <div className="text-center mb-8">
          <h1 className="text-3xl font-extrabold tracking-tight mb-2 bg-clip-text text-transparent bg-gradient-to-r from-emerald-400 to-indigo-400" style={{
            background: 'linear-gradient(135deg, #34d399 0%, #818cf8 100%)',
            WebkitBackgroundClip: 'text',
            WebkitTextFillColor: 'transparent'
          }}>
            Controle de Dieta
          </h1>
          <p className="text-sm text-gray-400">
            {isLogin ? 'Faça login para monitorar sua dieta com IA' : 'Crie sua conta para começar seu plano alimentar'}
          </p>
        </div>

        {/* Tab Headers */}
        <div className="flex border-b border-gray-800 mb-6">
          <button
            type="button"
            className={`flex-1 pb-3 text-center font-medium transition-all ${
              isLogin ? 'text-emerald-400 border-b-2 border-emerald-500' : 'text-gray-400 hover:text-gray-200'
            }`}
            onClick={() => {
              setIsLogin(true);
              setError('');
              setSuccess('');
            }}
          >
            Entrar
          </button>
          <button
            type="button"
            className={`flex-1 pb-3 text-center font-medium transition-all ${
              !isLogin ? 'text-emerald-400 border-b-2 border-emerald-500' : 'text-gray-400 hover:text-gray-200'
            }`}
            onClick={() => {
              setIsLogin(false);
              setError('');
              setSuccess('');
            }}
          >
            Cadastrar
          </button>
        </div>

        {/* Errors & Success */}
        {error && (
          <div className="alert alert-error">
            <AlertCircle size={18} className="shrink-0" />
            <span>{error}</span>
          </div>
        )}
        {success && (
          <div className="alert alert-success">
            <CheckCircle2 size={18} className="shrink-0" />
            <span>{success}</span>
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          {!isLogin && (
            <div className="form-group">
              <label className="form-label" htmlFor="username">Apelido / Usuário</label>
              <div className="relative">
                <span className="absolute inset-y-0 left-0 pl-3 flex items-center text-gray-500">
                  <User size={18} />
                </span>
                <input
                  id="username"
                  type="text"
                  required
                  placeholder="Seu nome de usuário"
                  className="form-input pl-10"
                  value={userName}
                  onChange={(e) => setUserName(e.target.value)}
                />
              </div>
            </div>
          )}

          <div className="form-group">
            <label className="form-label" htmlFor="email">E-mail</label>
            <div className="relative">
              <span className="absolute inset-y-0 left-0 pl-3 flex items-center text-gray-500">
                <Mail size={18} />
              </span>
              <input
                id="email"
                type="email"
                required
                placeholder="exemplo@dieta.com"
                className="form-input pl-10"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>
          </div>

          <div className="form-group">
            <label className="form-label" htmlFor="password">Senha</label>
            <div className="relative">
              <span className="absolute inset-y-0 left-0 pl-3 flex items-center text-gray-500">
                <Lock size={18} />
              </span>
              <input
                id="password"
                type="password"
                required
                placeholder="Sua senha"
                className="form-input pl-10"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>
          </div>

          <button
            type="submit"
            disabled={loading}
            className="btn btn-primary w-full mt-6"
          >
            {loading ? (
              <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin"></div>
            ) : isLogin ? (
              <>
                <LogIn size={18} />
                <span>Entrar</span>
              </>
            ) : (
              <>
                <UserPlus size={18} />
                <span>Cadastrar</span>
              </>
            )}
          </button>
        </form>
      </div>
    </div>
  );
}
