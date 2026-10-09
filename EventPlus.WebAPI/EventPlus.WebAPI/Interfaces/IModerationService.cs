namespace EventPlus.WebAPI.Interfaces
{
    public interface IModerationService
    {
        //Retorna true se o texto foi reprovado (flagged) pela moderação
        Task<bool> ModerarTexto(string texto);
    }
}
