using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

public partial class Instituicao
{
    [Key]
    public Guid IdInstituicao { get; set; }

    [Column("CNPJ")]
    [StringLength(14)]
    [Unicode(false)]
    public string Cnpj { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string NomeFantasia { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Endereco { get; set; } = null!;

    [InverseProperty("IdInstituicaoNavigation")]
    public virtual ICollection<Evento> Evento { get; set; } = new List<Evento>();
}
