using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class PresencaDTO
    {
        public Guid? IdUsuario { get; set; }

        public Guid? IdEvento { get; set; }
        public bool situacao { get; set; }
    }
}
