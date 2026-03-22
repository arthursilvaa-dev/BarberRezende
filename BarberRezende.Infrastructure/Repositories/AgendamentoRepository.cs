using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BarberRezende.Infrastructure.Repositories
{
    /// <summary>
    /// Repositório concreto de Agendamentos (Infrastructure).
    /// Aqui ficam queries específicas e eficientes para validações de agenda.
    /// </summary>
    public class AgendamentoRepository : GenericRepository<Agendamento>, IAgendamentoRepository
    {
        // Guardamos o contexto concreto para queries com Include/ToListAsync
        private readonly BarberRezendeDbContext _context;

        public AgendamentoRepository(BarberRezendeDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca agendamentos do barbeiro no range [start, end),
        /// já carregando o Serviço para termos DuracaoMinutos.
        /// </summary>
        public async Task<List<Agendamento>> GetByBarbeiroInRangeWithServicoAsync(int barbeiroId, DateTime start, DateTime end)
        {
            return await _context.Agendamentos
                .AsNoTracking()
                .Include(a => a.Servico)
                .Where(a =>
                    a.BarbeiroId == barbeiroId &&
                    a.DataHora >= start &&
                    a.DataHora < end
                )
                .ToListAsync();
        }

        /// <summary>
        /// Valida sobreposição de horários considerando duração do serviço.
        /// Overlap:
        /// existenteStart < novoEnd  &&  existenteEnd > novoStart
        /// </summary>
        public async Task<bool> ExistsOverlappingAsync(int barbeiroId, DateTime start, DateTime end, int? ignoreAgendamentoId = null)
        {
            // 1) Query base: todos os agendamentos do barbeiro, com Serviço carregado.
            //    Precisa do Include para pegar DuracaoMinutos.
            var query = _context.Agendamentos
                .AsNoTracking()
                .Include(a => a.Servico)
                .Where(a => a.BarbeiroId == barbeiroId);

            // 2) Em UPDATE, ignoramos o próprio registro para não "conflitar consigo mesmo".
            if (ignoreAgendamentoId.HasValue)
                query = query.Where(a => a.Id != ignoreAgendamentoId.Value);

            // 3) Traz para memória: vamos calcular o "End" com base na duração.
            //    (EF nem sempre traduz bem esse cálculo quando depende de navegação carregada.)
            var existentes = await query.ToListAsync();

            // 4) Verifica overlap com duração real do serviço
            foreach (var ag in existentes) {
                // Segurança: se por algum motivo Servico não carregou, não podemos validar corretamente.
                // Nesse caso, é melhor impedir (fail safe) do que permitir conflito.
                var duracao = ag.Servico?.DuracaoMinutos ?? 0;
                if (duracao <= 0)
                    return true;

                var existenteStart = ag.DataHora;
                var existenteEnd = existenteStart.AddMinutes(duracao);

                // Overlap padrão (matemática de intervalos)
                var overlap = existenteStart < end && existenteEnd > start;
                if (overlap)
                    return true;
            }

            return false;
        }
    }
}
