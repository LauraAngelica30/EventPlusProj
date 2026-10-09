using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IInstituicao
    {
        Task Cadastrar(Instituicao instituicao);

        Task<List<Instituicao>> Listar();

        Task Atualizar(Guid id, Instituicao instituicao);

        Task Deletar(Guid id);

        Task<Instituicao?> BuscarPorId(Guid id);

    }
}
