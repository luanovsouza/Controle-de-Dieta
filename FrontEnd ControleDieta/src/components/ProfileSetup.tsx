import React, { useState } from 'react';
import { User, Activity, Target, Ruler, Scale, Calendar, ChevronRight, AlertCircle } from 'lucide-react';
import { apiFetch } from '../services/api';

interface ProfileSetupProps {
  onProfileComplete: (metaDiaria: number, name: string) => void;
}

export default function ProfileSetup({ onProfileComplete }: ProfileSetupProps) {
  const [nome, setNome] = useState('');
  const [idade, setIdade] = useState<number | ''>('');
  const [sexo, setSexo] = useState('Masculino');
  const [peso, setPeso] = useState<number | ''>('');
  const [altura, setAltura] = useState<number | ''>('');
  const [atividadeFisica, setAtividadeFisica] = useState('Sedentario');
  const [meta, setMeta] = useState('Emagrecer');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!nome || !idade || !peso || !altura) {
      setError('Por favor, preencha todos os campos obrigatórios.');
      return;
    }

    setError('');
    setLoading(true);

    try {
      const response = await apiFetch<{
        mensagem: string;
        meta: number;
        dadosFisicos: {
          nome: string;
          metaDiaria: number;
        };
      }>('/api/UserMetaCal/calcular-meta-diaria', {
        method: 'POST',
        body: JSON.stringify({
          nome,
          idade: Number(idade),
          sexo,
          atividadeFisica,
          peso: Number(peso),
          altura: Number(altura),
          meta
        })
      });

      if (response && response.meta !== undefined) {
        onProfileComplete(response.meta, response.dadosFisicos?.nome || nome);
      } else {
        throw new Error('Falha ao receber a meta diária calculada.');
      }
    } catch (err: any) {
      console.error(err);
      setError(err.message || 'Erro ao calcular meta diária. Verifique seus dados.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex-1 flex items-center justify-center p-6 min-h-[85vh]">
      <div className="w-full max-w-lg glass-panel relative">
        <div className="absolute top-0 left-0 right-0 h-[4px] bg-gradient-to-r from-emerald-500 to-indigo-500" style={{ background: 'linear-gradient(90deg, #10b981 0%, #6366f1 100%)' }}></div>
        
        <div className="mb-6">
          <h2 className="text-2xl font-bold mb-2">Configurar Perfil Metabólico</h2>
          <p className="text-sm text-gray-400">
            Precisamos de algumas informações físicas para calcular sua Taxa Metabólica Basal (TMB) e definir suas metas diárias.
          </p>
        </div>

        {error && (
          <div className="alert alert-error">
            <AlertCircle size={18} className="shrink-0" />
            <span>{error}</span>
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {/* Nome */}
            <div className="form-group md:col-span-2">
              <label className="form-label" htmlFor="nome">Nome Completo</label>
              <div className="relative">
                <span className="absolute inset-y-0 left-0 pl-3 flex items-center text-gray-500">
                  <User size={18} />
                </span>
                <input
                  id="nome"
                  type="text"
                  required
                  placeholder="Como gostaria de ser chamado?"
                  className="form-input pl-10"
                  value={nome}
                  onChange={(e) => setNome(e.target.value)}
                />
              </div>
            </div>

            {/* Idade */}
            <div className="form-group">
              <label className="form-label" htmlFor="idade">Idade (anos)</label>
              <div className="relative">
                <span className="absolute inset-y-0 left-0 pl-3 flex items-center text-gray-500">
                  <Calendar size={18} />
                </span>
                <input
                  id="idade"
                  type="number"
                  required
                  min="1"
                  max="100"
                  placeholder="Ex: 28"
                  className="form-input pl-10"
                  value={idade}
                  onChange={(e) => setIdade(e.target.value === '' ? '' : Number(e.target.value))}
                />
              </div>
            </div>

            {/* Sexo */}
            <div className="form-group">
              <label className="form-label" htmlFor="sexo">Gênero</label>
              <select
                id="sexo"
                className="form-select"
                value={sexo}
                onChange={(e) => setSexo(e.target.value)}
              >
                <option value="Masculino">Masculino</option>
                <option value="Feminino">Feminino</option>
              </select>
            </div>

            {/* Peso */}
            <div className="form-group">
              <label className="form-label" htmlFor="peso">Peso Atual (kg)</label>
              <div className="relative">
                <span className="absolute inset-y-0 left-0 pl-3 flex items-center text-gray-500">
                  <Scale size={18} />
                </span>
                <input
                  id="peso"
                  type="number"
                  step="0.1"
                  required
                  placeholder="Ex: 75.5"
                  className="form-input pl-10"
                  value={peso}
                  onChange={(e) => setPeso(e.target.value === '' ? '' : Number(e.target.value))}
                />
              </div>
            </div>

            {/* Altura */}
            <div className="form-group">
              <label className="form-label" htmlFor="altura">Altura (cm)</label>
              <div className="relative">
                <span className="absolute inset-y-0 left-0 pl-3 flex items-center text-gray-500">
                  <Ruler size={18} />
                </span>
                <input
                  id="altura"
                  type="number"
                  required
                  placeholder="Ex: 175"
                  className="form-input pl-10"
                  value={altura}
                  onChange={(e) => setAltura(e.target.value === '' ? '' : Number(e.target.value))}
                />
              </div>
            </div>

            {/* Atividade Física */}
            <div className="form-group">
              <label className="form-label" htmlFor="atividade">Nível de Atividade</label>
              <div className="relative">
                <select
                  id="atividade"
                  className="form-select"
                  value={atividadeFisica}
                  onChange={(e) => setAtividadeFisica(e.target.value)}
                >
                  <option value="Sedentario">Sedentário (Sem exercícios)</option>
                  <option value="Leve">Leve (1-3 dias/semana)</option>
                  <option value="Moderado">Moderado (3-5 dias/semana)</option>
                  <option value="Intenso">Intenso (6-7 dias/semana)</option>
                </select>
              </div>
            </div>

            {/* Meta */}
            <div className="form-group">
              <label className="form-label" htmlFor="meta">Objetivo da Dieta</label>
              <select
                id="meta"
                className="form-select"
                value={meta}
                onChange={(e) => setMeta(e.target.value)}
              >
                <option value="Emagrecer">Emagrecer (Déficit Calórico)</option>
                <option value="ManterPeso">Manter Peso</option>
                <option value="GanharMassa">Ganhar Massa (Superávit Calórico)</option>
              </select>
            </div>
          </div>

          <button
            type="submit"
            disabled={loading}
            className="btn btn-primary w-full mt-6"
          >
            {loading ? (
              <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin"></div>
            ) : (
              <>
                <span>Calcular & Continuar</span>
                <ChevronRight size={18} />
              </>
            )}
          </button>
        </form>
      </div>
    </div>
  );
}
