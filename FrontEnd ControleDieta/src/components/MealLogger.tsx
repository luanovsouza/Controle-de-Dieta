import React, { useState } from 'react';
import { Send, Sparkles, Brain, Plus, AlertCircle, ArrowRight } from 'lucide-react';
import { apiFetch } from '../services/api';

interface MealLoggerProps {
  userId: number;
  onMealLogged: () => void;
}

interface ParsedMeal {
  calories: number;
  protein: number;
  carbs: number;
  fat: number;
}

export default function MealLogger({ userId, onMealLogged }: MealLoggerProps) {
  const [description, setDescription] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [result, setResult] = useState<ParsedMeal | null>(null);
  const [lastLoggedDescription, setLastLoggedDescription] = useState('');

  const handleParseMeal = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!description.trim()) return;

    setError('');
    setResult(null);
    setLoading(true);
    const currentDesc = description;

    try {
      const response = await apiFetch<{
        message: string;
        resultado: ParsedMeal;
      }>('/api/ProcessFoodIa/refeicao', {
        method: 'POST',
        body: JSON.stringify({
          descricao: currentDesc,
          userId: userId
        })
      });

      if (response && response.resultado) {
        setResult(response.resultado);
        setLastLoggedDescription(currentDesc);
        setDescription('');
        onMealLogged(); // Refresh parent dashboard stats
      } else {
        throw new Error('Não foi possível extrair os macronutrientes da refeição.');
      }
    } catch (err: any) {
      console.error(err);
      setError(err.message || 'Erro de comunicação com a Inteligência Artificial local (Ollama). Verifique se o serviço está de pé.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="glass-panel relative overflow-hidden">
      <div className="absolute top-0 right-0 p-4 opacity-5 text-indigo-400">
        <Brain size={120} />
      </div>

      <div className="flex items-center gap-2 mb-4">
        <Sparkles className="text-indigo-400" size={20} />
        <h3 className="text-lg font-bold">Registrar Refeição com IA</h3>
      </div>

      <p className="text-sm text-gray-400 mb-4">
        Digite o que você comeu em linguagem natural e nossa Inteligência Artificial calculará as calorias e macronutrientes automaticamente.
      </p>

      {error && (
        <div className="alert alert-error mb-4">
          <AlertCircle size={18} className="shrink-0" />
          <span>{error}</span>
        </div>
      )}

      <form onSubmit={handleParseMeal} className="space-y-4">
        <div className="form-group">
          <textarea
            required
            rows={3}
            disabled={loading}
            placeholder="Ex: No almoço comi 2 colheres de arroz integral, 1 concha de feijão carioca e 150g de filé de frango grelhado. Tomei uma lata de refrigerante zero."
            className="form-textarea resize-none pr-10"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>

        <div className="flex justify-end">
          <button
            type="submit"
            disabled={loading || !description.trim()}
            className="btn btn-secondary"
          >
            {loading ? (
              <>
                <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin"></div>
                <span>Analisando Refeição...</span>
              </>
            ) : (
              <>
                <Send size={16} />
                <span>Analisar e Registrar</span>
              </>
            )}
          </button>
        </div>
      </form>

      {/* Shimmer/Skeleton loading state */}
      {loading && (
        <div className="mt-6 border border-gray-800 rounded-xl p-4 bg-black/20 space-y-4 animate-pulse-slow">
          <div className="h-4 bg-gray-800 rounded w-1/3 shimmer"></div>
          <div className="grid grid-cols-4 gap-2">
            {[1, 2, 3, 4].map((i) => (
              <div key={i} className="h-16 bg-gray-800 rounded shimmer"></div>
            ))}
          </div>
        </div>
      )}

      {/* Result Cards */}
      {result && !loading && (
        <div className="mt-6 border border-emerald-500/20 rounded-xl p-4 bg-emerald-950/5 relative animate-fade-in">
          <div className="absolute top-3 right-3 text-emerald-400 bg-emerald-400/10 rounded-full p-1">
            <Plus size={16} />
          </div>

          <h4 className="font-semibold text-sm text-gray-400 mb-1">
            Refeição Registrada!
          </h4>
          <p className="text-sm font-medium mb-4 italic text-gray-200">
            &ldquo;{lastLoggedDescription}&rdquo;
          </p>

          <div className="grid grid-cols-4 gap-2 text-center">
            {/* Calories Card */}
            <div className="bg-black/30 border border-gray-800 rounded-lg p-2">
              <span className="block text-xs text-gray-400">Calorias</span>
              <span className="block text-md font-bold text-emerald-400">{Math.round(result.calories)} kcal</span>
            </div>

            {/* Protein Card */}
            <div className="bg-black/30 border border-gray-800 rounded-lg p-2">
              <span className="block text-xs text-gray-400">Proteínas</span>
              <span className="block text-md font-bold text-indigo-400">{Math.round(result.protein)}g</span>
            </div>

            {/* Carbs Card */}
            <div className="bg-black/30 border border-gray-800 rounded-lg p-2">
              <span className="block text-xs text-gray-400">Carboidratos</span>
              <span className="block text-md font-bold text-amber-400">{Math.round(result.carbs)}g</span>
            </div>

            {/* Fat Card */}
            <div className="bg-black/30 border border-gray-800 rounded-lg p-2">
              <span className="block text-xs text-gray-400">Gorduras</span>
              <span className="block text-md font-bold text-rose-400">{Math.round(result.fat)}g</span>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
