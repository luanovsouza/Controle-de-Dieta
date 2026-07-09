import React, { useState } from 'react';
import { ChefHat, Sparkles, Loader2, UtensilsCrossed, AlertCircle, RotateCcw, Flame, Dumbbell } from 'lucide-react';
import { apiFetch } from '../services/api';

interface Recipe {
  nome: string;
  ingredientes: string[];
  modoDePreparo: string;
  calorias: number;
  gordura: number;
  proteinas: number;
}

export default function RecipeGenerator() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [recipe, setRecipe] = useState<Recipe | null>(null);

  const handleGenerate = async () => {
    setError('');
    setLoading(true);

    try {
      const response = await apiFetch<Recipe>('/ReceitasIa/gerar-receita', {
        method: 'POST'
      });

      if (response && response.nome) {
        setRecipe(response);
      } else {
        throw new Error('Resposta inválida da IA ao gerar receita.');
      }
    } catch (err: any) {
      console.error(err);
      setError(err.message || 'Erro ao gerar receita. Verifique se o Ollama está rodando.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="glass-panel relative overflow-hidden">
      <div className="absolute top-0 right-0 p-4 opacity-5 text-purple-400">
        <ChefHat size={120} />
      </div>

      <div className="flex items-center gap-2 mb-2">
        <Sparkles className="text-purple-400" size={20} />
        <h3 className="text-lg font-bold">Receita Personalizada com IA</h3>
      </div>

      <p className="text-sm text-gray-400 mb-4">
        Gere uma receita saudável e brasileira baseada na sua meta calórica diária.
      </p>

      {error && (
        <div className="alert alert-error mb-4">
          <AlertCircle size={18} className="shrink-0" />
          <span>{error}</span>
        </div>
      )}

      <button
        onClick={handleGenerate}
        disabled={loading}
        className="btn btn-secondary w-full"
      >
        {loading ? (
          <>
            <Loader2 size={18} className="animate-spin" />
            <span>Criando sua receita...</span>
          </>
        ) : (
          <>
            <ChefHat size={18} />
            <span>{recipe ? 'Gerar Nova Receita' : 'Gerar Receita'}</span>
          </>
        )}
      </button>

      {/* Skeleton while loading */}
      {loading && (
        <div className="mt-6 space-y-3 animate-pulse-slow">
          <div className="h-5 bg-gray-800 rounded w-1/2 shimmer"></div>
          <div className="h-3 bg-gray-800 rounded w-full shimmer"></div>
          <div className="h-3 bg-gray-800 rounded w-5/6 shimmer"></div>
          <div className="h-3 bg-gray-800 rounded w-4/6 shimmer"></div>
          <div className="h-24 bg-gray-800 rounded shimmer mt-4"></div>
        </div>
      )}

      {recipe && !loading && (
        <div className="mt-6 space-y-4">
          {/* Recipe Header */}
          <div className="flex items-start justify-between gap-4 pb-3 border-b border-gray-800">
            <div className="flex items-center gap-2">
              <UtensilsCrossed className="text-purple-400 shrink-0" size={20} />
              <h4 className="text-xl font-bold text-purple-300">{recipe.nome}</h4>
            </div>
            <button
              onClick={handleGenerate}
              className="text-gray-500 hover:text-gray-300 transition-colors"
              title="Gerar nova receita"
            >
              <RotateCcw size={16} />
            </button>
          </div>

          {/* Macro Badges */}
          <div className="flex flex-wrap gap-2">
            <span className="flex items-center gap-1 text-xs px-3 py-1 rounded-full bg-orange-500/10 text-orange-400 border border-orange-500/20">
              <Flame size={12} /> {recipe.calorias} kcal
            </span>
            <span className="flex items-center gap-1 text-xs px-3 py-1 rounded-full bg-indigo-500/10 text-indigo-400 border border-indigo-500/20">
              <Dumbbell size={12} /> {recipe.proteinas}g proteína
            </span>
            <span className="flex items-center gap-1 text-xs px-3 py-1 rounded-full bg-rose-500/10 text-rose-400 border border-rose-500/20">
              {recipe.gordura}g gordura
            </span>
          </div>

          {/* Ingredients */}
          <div>
            <h5 className="text-sm font-semibold text-gray-300 mb-2 uppercase tracking-wider">Ingredientes</h5>
            <ul className="space-y-1">
              {Array.isArray(recipe.ingredientes) ? recipe.ingredientes.map((ing, i) => (
                <li key={i} className="text-sm text-gray-400 flex items-start gap-2">
                  <span className="text-emerald-500 mt-0.5 shrink-0">▸</span>
                  {ing}
                </li>
              )) : (
                <li className="text-sm text-gray-400">{String(recipe.ingredientes)}</li>
              )}
            </ul>
          </div>

          {/* Instructions */}
          <div>
            <h5 className="text-sm font-semibold text-gray-300 mb-2 uppercase tracking-wider">Modo de Preparo</h5>
            <p className="text-sm text-gray-400 leading-relaxed">
              {recipe.modoDePreparo}
            </p>
          </div>
        </div>
      )}
    </div>
  );
}
