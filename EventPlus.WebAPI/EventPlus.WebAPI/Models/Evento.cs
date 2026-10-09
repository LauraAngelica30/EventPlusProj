using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

public partial class Evento
{
    [Key]
    public Guid IdEvento { get; set; }

    public Guid? IdTipoEvento { get; set; }

    public Guid? IdInstituicao { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string NomeEvento { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Descricao { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DataEvento { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string ImagemUrl { get; set; } = null!;

    [InverseProperty("IdEventoNavigation")]
    public virtual ICollection<Comentario> Comentario { get; set; } = new List<Comentario>();

    [ForeignKey("IdInstituicao")]
    [InverseProperty("Evento")]
    public virtual Instituicao? IdInstituicaoNavigation { get; set; }

    [ForeignKey("IdTipoEvento")]
    [InverseProperty("Evento")]
    public virtual TipoEvento? IdTipoEventoNavigation { get; set; }

    [InverseProperty("IdEventoNavigation")]
    public virtual ICollection<Presenca> Presenca { get; set; } = new List<Presenca>();
}
