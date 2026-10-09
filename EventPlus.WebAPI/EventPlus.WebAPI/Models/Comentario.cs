using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

public partial class Comentario
{
    [Key]
    public Guid IdComentario { get; set; }

    public Guid? IdUsuario { get; set; }

    public Guid? IdEvento { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DataComentario { get; set; }

    public bool Exibe { get; set; }

    [ForeignKey("IdEvento")]
    [InverseProperty("Comentario")]
    public virtual Evento? IdEventoNavigation { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Comentario")]
    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
