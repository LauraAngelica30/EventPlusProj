using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class ComentarioRepository : IComentario
    {
        private readonly EventContext _context;

        public ComentarioRepository(EventContext context)
        {
            _context = context;
        }
        public async Task<Comentario?> BuscarPorId(Guid id)
        {
            return await _context.Comentario.FirstOrDefaultAsync(t => t.IdComentario == id);
        }

        public async Task Cadastrar(Comentario comentario)
        {
            comentario.DataComentario = DateTime.Now;
            await _context.Comentario.AddAsync(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var comentarioBuscado = await _context.Comentario.FindAsync(id);

            if (comentarioBuscado != null)
            {
                _context.Comentario.Remove(comentarioBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Comentario>> Listar()
        {
            return await _context.Comentario.AsNoTracking().ToListAsync();
        }

        public async Task<List<Comentario>> ListarPorEvento(Guid idEvento)
        {
            return await _context.Comentario.Where(x => x.IdEvento == idEvento).ToListAsync();
        }
    }
}
