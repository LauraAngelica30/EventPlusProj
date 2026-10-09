using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IEvento
    {
        Task Cadastrar(Evento evento);

        Task<List<Evento>> Listar();

        Task Atualizar(Guid id, Evento evento);
        
        Task Deletar(Guid id);

        Task<Evento?> BuscarPorId(Guid id);

        Task<List<Evento>> ListarPorInstituicao(Guid idInstituicao);

        Task<List<Evento>> ListarPorInscrito(Guid id);

        Task<List<Evento>> ListarProximosEventos(Guid idEvento);
    }
}
