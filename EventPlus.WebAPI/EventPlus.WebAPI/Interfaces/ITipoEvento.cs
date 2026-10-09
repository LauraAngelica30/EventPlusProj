using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    /// <summary>
    /// Interface do repositório para a entidade TipoEvento
    /// Contrato da de TipoEvento, Métodos que deverão ser implementados dentro do repositório
    /// </summary>
    public interface ITipoEvento
    {
        Task CadastrarTipoEvento(TipoEvento tipoEvento);

        Task<List<TipoEvento>> ListarTipoEvento();

        Task AtualizarTipoEvento(Guid id, TipoEvento tipoEvento);

        Task DeletarTipoEvento(Guid id);

        Task<TipoEvento?> BuscarTipoEventoPorId(Guid id);
    }
}
