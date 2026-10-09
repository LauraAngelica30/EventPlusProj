using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IPresenca
    {
        Task Increver(Presenca presenca);

        Task<List<Presenca>> Listar();

        Task<List<Presenca>> ListarMinhas(Guid idUsuario);

        Task AtualizarSituacao(Guid id, Presenca presenca);

        Task Deletar(Guid id);

        Task<Presenca?> BuscarPorId(Guid id);
    }
}
