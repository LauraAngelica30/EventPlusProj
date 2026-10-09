using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class TipoEventoRepository : ITipoEvento

    {
        private readonly EventContext _context;

        public TipoEventoRepository(EventContext context)
        {
            _context = context;
        }

        public async Task AtualizarTipoEvento(Guid id, TipoEvento tipoEvento)
        {
            var tipoEventoBuscado = await _context.TipoEvento.FindAsync(id);
            if (tipoEventoBuscado != null)
            {
                tipoEventoBuscado.TituloTipoEvento = tipoEvento.TituloTipoEvento;

                _context.TipoEvento.Update(tipoEventoBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TipoEvento?> BuscarTipoEventoPorId(Guid id)
        {
            return await _context.TipoEvento.FirstOrDefaultAsync(tE => tE.IdTipoEvento == id);
        }

        public async Task CadastrarTipoEvento(TipoEvento tipoEvento)
        {
            await _context.TipoEvento.AddAsync(tipoEvento);

            await _context.SaveChangesAsync();
        }

        public async Task DeletarTipoEvento(Guid id)
        {
            var tipoEventoBuscado = await _context.TipoEvento.FindAsync(id);

            if (tipoEventoBuscado != null)
            {
                _context.TipoEvento.Remove(tipoEventoBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TipoEvento>> ListarTipoEvento()
        {
            return await _context.TipoEvento.AsNoTracking().ToListAsync();
        }
    }
}
