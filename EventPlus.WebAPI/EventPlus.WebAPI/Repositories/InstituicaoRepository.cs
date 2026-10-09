using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class InstituicaoRepository : IInstituicao
    {
        private readonly EventContext _context;

        public InstituicaoRepository(EventContext context)
        {
            _context = context;
        }

        public async Task Atualizar(Guid id, Instituicao instituicao)
        {
            var instituicaoBuscada = await _context.Instituicao.FindAsync(id);

            if (instituicaoBuscada != null)
            {
                instituicaoBuscada.NomeFantasia = instituicao.NomeFantasia;
                instituicaoBuscada.Cnpj = instituicao.Cnpj;
                instituicaoBuscada.Endereco = instituicao.Endereco;

                _context.Instituicao.Update(instituicaoBuscada);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Instituicao?> BuscarPorId(Guid id)
        {
            return await _context.Instituicao.FirstOrDefaultAsync(t => t.IdInstituicao == id);
        }

        public async Task Cadastrar(Instituicao instituicao)
        {
            await _context.Instituicao.AddAsync(instituicao);

            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var instituicaoBuscada = await _context.Instituicao.FindAsync(id);

            if (instituicaoBuscada != null)
            {
                _context.Instituicao.Remove(instituicaoBuscada);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Instituicao>> Listar()
        {
            return await _context.Instituicao.AsNoTracking().ToListAsync();
        }
    }
}
