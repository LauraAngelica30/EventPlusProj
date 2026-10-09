using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

[Index("Email", Name = "UQ__Usuario__A9D105344013094F", IsUnique = true)]
public partial class Usuario
{
    [Key]
    public Guid IdUsuario { get; set; }

    public Guid? IdTipoUsuario { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Nome { get; set; }

    [StringLength(256)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(60)]
    [Unicode(false)]
    [JsonIgnore]
    public string? Senha { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Comentario> Comentario { get; set; } = new List<Comentario>();

    [ForeignKey("IdTipoUsuario")]
    [InverseProperty("Usuario")]
    public virtual TipoUsuario? IdTipoUsuarioNavigation { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Presenca> Presenca { get; set; } = new List<Presenca>();
}
