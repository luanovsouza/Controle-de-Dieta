import React, { useState, useEffect, useCallback } from 'react';
import {
  Flame, Beef, Wheat, Droplets, UtensilsCrossed,
  Clock, Calendar, TrendingUp, LogOut, RefreshCw, AlertCircle
} from 'lucide-react';
import { apiFetch, authService } from '../services/api';
import MealLogger from './MealLogger';
import RecipeGenerator from './RecipeGenerator';

interface DashboardProps {
  userId: number;
  userName: string;
  metaDiaria: number;
  onLogout: () => void;
}

interface Meal {
  id: number;
  description: string;
  calories: number;
  protein: number;
  carbs: number;
  fat: number;
  consumedAt: string;
}

interface MacroTotals {
  calories: number;
  protein: number;
  carbs: number;
  fat: number;
}

function MacroBar({ label, value, max, color, unit }: { label: string; value: number; max: number; color: string; unit: string }) {
  const pct = max > 0 ? Math.min((value / max) * 100, 100) : 0;
  return (
    <div>
      <div className="flex justify-between text-xs mb-1">
        <span className="text-gray-400">{label}</span>
        <span className="font-medium" style={{ color }}>{Math.round(value)}{unit}</span>
      </div>
      <div className="progress-container">
        <div className="progress-bar" style={{ width: `${pct}%`, background: color }} />
      </div>
    </div>
  );
}

export default function Dashboard({ userId, userName, metaDiaria, onLogout }: DashboardProps) {
  const [meals, setMeals] = useState<Meal[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [activeTab, setActiveTab] = useState<'logger' | 'recipe'>('logger');

  const fetchMeals = useCallback(async () => {
    setLoading(true);
    setError('');
    try {
      const data = await apiFetch<Meal[]>(`/api/Refeicoes/${userId}`);
      setMeals(Array.isArray(data) ? data : []);
    } catch (err: any) {
      setError('Não foi possível carregar o histórico de refeições.');
    } finally {
      setLoading(false);
    }
  }, [userId]);

  useEffect(() => {
    fetchMeals();
  }, [fetchMeals]);

  // Calculate macro totals for today
  const today = new Date().toISOString().slice(0, 10);
  const todayMeals = meals.filter(m => m.consumedAt?.slice(0, 10) === today);

  const totals: MacroTotals = todayMeals.reduce(
    (acc, m) => ({
      calories: acc.calories + (m.calories || 0),
      protein: acc.protein + (m.protein || 0),
      carbs: acc.carbs + (m.carbs || 0),
      fat: acc.fat + (m.fat || 0),
    }),
    { calories: 0, protein: 0, carbs: 0, fat: 0 }
  );

  const pctConsumed = metaDiaria > 0 ? Math.min((totals.calories / metaDiaria) * 100, 100) : 0;
  const remaining = Math.max(metaDiaria - totals.calories, 0);

  // Estimated macros from target (protein = 30%, carbs = 50%, fat = 20%)
  const targetProtein = (metaDiaria * 0.30) / 4;
  const targetCarbs = (metaDiaria * 0.50) / 4;
  const targetFat = (metaDiaria * 0.20) / 9;

  return (
    <div className="w-full min-h-screen flex flex-col" style={{ backgroundColor: 'hsl(240 10% 4%)' }}>
      {/* Header */}
      <header className="sticky top-0 z-50 border-b border-gray-800/60 backdrop-blur-xl bg-black/30">
        <div className="max-w-6xl mx-auto px-4 py-3 flex items-center justify-between">
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-full flex items-center justify-center text-white text-sm font-bold"
              style={{ background: 'linear-gradient(135deg, #10b981, #6366f1)' }}>
              {userName.charAt(0).toUpperCase()}
            </div>
            <div>
              <p className="text-xs text-gray-400">Bem-vindo de volta,</p>
              <p className="text-sm font-semibold">{userName}</p>
            </div>
          </div>
          <div className="flex items-center gap-2 text-sm font-medium text-emerald-400 bg-emerald-400/10 px-3 py-1 rounded-full border border-emerald-400/20">
            <Flame size={14} />
            <span>Meta: {Math.round(metaDiaria)} kcal/dia</span>
          </div>
          <button onClick={onLogout} className="btn btn-outline text-xs gap-1 px-3 py-2">
            <LogOut size={14} />
            <span>Sair</span>
          </button>
        </div>
      </header>

      <main className="max-w-6xl mx-auto w-full px-4 py-6 flex-1">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">

          {/* LEFT COLUMN — Calorie Ring + Macro Bars */}
          <div className="flex flex-col gap-4">
            {/* Calorie Ring Card */}
            <div className="glass-panel text-center">
              <h3 className="text-sm font-semibold text-gray-400 uppercase tracking-wider mb-4">Hoje</h3>

              {/* SVG Donut */}
              <div className="flex justify-center mb-4">
                <svg width="140" height="140" viewBox="0 0 140 140">
                  <circle cx="70" cy="70" r="55" fill="none" stroke="rgba(255,255,255,0.05)" strokeWidth="16" />
                  <circle
                    cx="70" cy="70" r="55"
                    fill="none"
                    stroke="url(#grad)"
                    strokeWidth="16"
                    strokeLinecap="round"
                    strokeDasharray={`${2 * Math.PI * 55}`}
                    strokeDashoffset={`${2 * Math.PI * 55 * (1 - pctConsumed / 100)}`}
                    transform="rotate(-90 70 70)"
                    style={{ transition: 'stroke-dashoffset 0.8s ease' }}
                  />
                  <defs>
                    <linearGradient id="grad" x1="0%" y1="0%" x2="100%" y2="0%">
                      <stop offset="0%" stopColor="#10b981" />
                      <stop offset="100%" stopColor="#6366f1" />
                    </linearGradient>
                  </defs>
                  <text x="70" y="65" textAnchor="middle" fill="white" fontSize="20" fontWeight="bold">
                    {Math.round(totals.calories)}
                  </text>
                  <text x="70" y="82" textAnchor="middle" fill="#6b7280" fontSize="10">
                    kcal consumidas
                  </text>
                </svg>
              </div>

              <div className="grid grid-cols-2 gap-2 text-center text-sm">
                <div className="bg-black/20 rounded-lg p-2">
                  <p className="text-gray-500 text-xs">Meta</p>
                  <p className="font-bold text-white">{Math.round(metaDiaria)} kcal</p>
                </div>
                <div className="bg-black/20 rounded-lg p-2">
                  <p className="text-gray-500 text-xs">Restante</p>
                  <p className="font-bold text-emerald-400">{Math.round(remaining)} kcal</p>
                </div>
              </div>
            </div>

            {/* Macro Bars */}
            <div className="glass-panel space-y-4">
              <h3 className="text-sm font-semibold text-gray-400 uppercase tracking-wider">Macronutrientes</h3>
              <MacroBar label="Proteínas" value={totals.protein} max={targetProtein} color="#818cf8" unit="g" />
              <MacroBar label="Carboidratos" value={totals.carbs} max={targetCarbs} color="#f59e0b" unit="g" />
              <MacroBar label="Gorduras" value={totals.fat} max={targetFat} color="#f43f5e" unit="g" />
            </div>

            {/* Stats mini cards */}
            <div className="grid grid-cols-2 gap-3">
              <div className="glass-card text-center">
                <Beef size={18} className="mx-auto text-indigo-400 mb-1" />
                <p className="text-xs text-gray-500">Proteínas</p>
                <p className="font-bold text-sm text-indigo-300">{Math.round(totals.protein)}g</p>
              </div>
              <div className="glass-card text-center">
                <Wheat size={18} className="mx-auto text-amber-400 mb-1" />
                <p className="text-xs text-gray-500">Carboidratos</p>
                <p className="font-bold text-sm text-amber-300">{Math.round(totals.carbs)}g</p>
              </div>
              <div className="glass-card text-center">
                <Droplets size={18} className="mx-auto text-rose-400 mb-1" />
                <p className="text-xs text-gray-500">Gorduras</p>
                <p className="font-bold text-sm text-rose-300">{Math.round(totals.fat)}g</p>
              </div>
              <div className="glass-card text-center">
                <TrendingUp size={18} className="mx-auto text-emerald-400 mb-1" />
                <p className="text-xs text-gray-500">Refeições</p>
                <p className="font-bold text-sm text-emerald-300">{todayMeals.length}</p>
              </div>
            </div>
          </div>

          {/* CENTER COLUMN — AI Tabs */}
          <div className="flex flex-col gap-4">
            {/* Tab switcher */}
            <div className="flex border border-gray-800 rounded-xl overflow-hidden">
              <button
                className={`flex-1 py-3 text-sm font-medium flex items-center justify-center gap-2 transition-all ${activeTab === 'logger' ? 'bg-indigo-600/20 text-indigo-300 border-r border-gray-800' : 'text-gray-500 hover:text-gray-300 border-r border-gray-800'}`}
                onClick={() => setActiveTab('logger')}
              >
                <UtensilsCrossed size={15} />
                Registrar Refeição
              </button>
              <button
                className={`flex-1 py-3 text-sm font-medium flex items-center justify-center gap-2 transition-all ${activeTab === 'recipe' ? 'bg-purple-600/20 text-purple-300' : 'text-gray-500 hover:text-gray-300'}`}
                onClick={() => setActiveTab('recipe')}
              >
                <Flame size={15} />
                Gerar Receita
              </button>
            </div>

            {activeTab === 'logger' && (
              <MealLogger userId={userId} onMealLogged={fetchMeals} />
            )}
            {activeTab === 'recipe' && (
              <RecipeGenerator />
            )}
          </div>

          {/* RIGHT COLUMN — Meal History */}
          <div className="flex flex-col gap-4">
            <div className="glass-panel flex-1">
              <div className="flex items-center justify-between mb-4">
                <div className="flex items-center gap-2">
                  <Calendar size={16} className="text-gray-400" />
                  <h3 className="text-sm font-semibold text-gray-400 uppercase tracking-wider">Histórico de Refeições</h3>
                </div>
                <button onClick={fetchMeals} className="text-gray-600 hover:text-gray-300 transition-colors" title="Atualizar">
                  <RefreshCw size={14} />
                </button>
              </div>

              {error && (
                <div className="alert alert-error text-xs">
                  <AlertCircle size={14} /> {error}
                </div>
              )}

              {loading ? (
                <div className="space-y-2">
                  {[1,2,3].map(i => (
                    <div key={i} className="h-14 rounded-lg shimmer bg-gray-800/40"></div>
                  ))}
                </div>
              ) : meals.length === 0 ? (
                <div className="text-center py-10 text-gray-600">
                  <UtensilsCrossed size={36} className="mx-auto mb-2 opacity-30" />
                  <p className="text-sm">Nenhuma refeição registrada ainda.</p>
                  <p className="text-xs mt-1">Use o registro com IA para começar!</p>
                </div>
              ) : (
                <div className="space-y-2 max-h-[60vh] overflow-y-auto pr-1">
                  {[...meals].reverse().map(meal => (
                    <div key={meal.id} className="glass-card p-3">
                      <div className="flex items-start justify-between gap-2">
                        <p className="text-xs text-gray-300 font-medium leading-snug line-clamp-2 flex-1">
                          {meal.description}
                        </p>
                        <span className="text-xs font-bold text-emerald-400 whitespace-nowrap">
                          {Math.round(meal.calories)} kcal
                        </span>
                      </div>
                      <div className="flex items-center gap-3 mt-2 text-xs text-gray-600">
                        <span className="text-indigo-500">{Math.round(meal.protein)}g prot</span>
                        <span className="text-amber-600">{Math.round(meal.carbs)}g carb</span>
                        <span className="text-rose-700">{Math.round(meal.fat)}g gord</span>
                        <span className="ml-auto flex items-center gap-1">
                          <Clock size={10} />
                          {new Date(meal.consumedAt).toLocaleDateString('pt-BR', { day: '2-digit', month: 'short', hour: '2-digit', minute: '2-digit' })}
                        </span>
                      </div>
                    </div>
                  ))}
                </div>
              )}
            </div>
          </div>

        </div>
      </main>
    </div>
  );
}
