using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IUsuario
    {
        Task Cadastrar(Usuario usuario);

        Task<List<Usuario>> Listar();

        Task Atualizar(Guid id, Usuario usuario);

        Task Deletar(Guid id);

        Task<Usuario?> BuscarPorId(Guid id);

        Task<Usuario?> BuscarPorEmailSenha(string email, string senha);
    }
}
