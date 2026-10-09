using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class InstituicaoDTO
    {
        [Required(ErrorMessage = "O nome da instituição é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da instituição pode ter no máximo 100 caracteres.")]
        public string NomeFantasia { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [StringLength(14, ErrorMessage = "Um CNPJ contém apenas 14 digitos")]
        public string CNPJ { get; set; }

        [Required(ErrorMessage = "O Endereço é obrigatório")]
        [StringLength(100, ErrorMessage = "O endereço pode ter no máximo 100 caracteres")]
        public string Endereco { get; set; }

    }
}
