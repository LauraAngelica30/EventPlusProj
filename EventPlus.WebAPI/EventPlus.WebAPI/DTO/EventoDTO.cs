using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class EventoDTO
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(100, ErrorMessage = "O nome do evento pode ter no máximo 100 caracteres")]
        public string NomeEvento { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime DataEvento { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Descricao { get; set; }

        public string? ImagemUrl { get; set; }

        public IFormFile? ArquivoImagem { get; set; }

        public Guid? IdTipoEvento { get; set; }
        public Guid? IdInstituicao { get; set; }

    }
}
