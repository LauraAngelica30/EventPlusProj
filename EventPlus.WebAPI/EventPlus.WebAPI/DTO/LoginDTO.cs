using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [EmailAddress(ErrorMessage = "Informe um email válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "A senha pode ter entre 8 e 60 caracteres")]
        public string Senha { get; set; } = string.Empty;
    }
}
