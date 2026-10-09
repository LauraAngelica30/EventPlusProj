using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

public partial class TipoEvento
{
    [Key]
    public Guid IdTipoEvento { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string TituloTipoEvento { get; set; } = null!;

    [InverseProperty("IdTipoEventoNavigation")]
    public virtual ICollection<Evento> Evento { get; set; } = new List<Evento>();
}
