using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class UsuarioRepository : IUsuario
    {
        private readonly EventContext _context;

        public UsuarioRepository(EventContext context) 
        {
           _context = context;
        }

        public async Task Atualizar(Guid id, Usuario usuario)
        {
            var usuarioBuscado = await _context.Usuario.FindAsync(id);
            if (usuarioBuscado != null)
            {
                usuarioBuscado.Nome = usuario.Nome;
                usuarioBuscado.Email = usuario.Email;
                usuarioBuscado.Senha = usuario.Senha;
                usuarioBuscado.IdTipoUsuario = usuario.IdTipoUsuario;

                _context.Usuario.Update(usuarioBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Usuario?> BuscarPorEmailSenha(string email, string senha)
        {
            //return await _context.Usuario.FirstOrDefaultAsync(e => e.Email == email && e.Senha == senha);
            var usuario = await _context.Usuario.Include(u => u.IdTipoUsuarioNavigation).FirstAsync(u => u.Email == email);

            if (usuario == null)
            {
                return null;
            }

            // Verifica se a senha digitada corresponde ao Hash salvo no banco
            bool senhaValida = Criptografia.CompararHash(senha, usuario.Senha);

            if (!senhaValida) // !: operador de negação
            {
                return null;
            }

            return usuario;

        }

        public async Task<Usuario?> BuscarPorId(Guid id)
        {
            return await _context.Usuario.FirstOrDefaultAsync(t => t.IdUsuario == id);
        }

        public async Task Cadastrar(Usuario usuario)
        {
            //Criptografamos a senha antes de salvar no banco 
            usuario.Senha = Criptografia.GerarHash(usuario.Senha);

            await _context.Usuario.AddAsync(usuario);

            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var usuarioBuscado = await _context.Usuario.FindAsync(id);

            if (usuarioBuscado != null)
            {
                _context.Usuario.Remove(usuarioBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Usuario>> Listar()
        {
            return await _context.Usuario.Include(u => u.IdTipoUsuarioNavigation).AsNoTracking().ToListAsync();
        }
    }
}
