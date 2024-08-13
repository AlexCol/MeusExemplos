using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExemploEntityBuscaModelsDaBase.src.models;

[Table("TB_CONFIG")]
[Index("Softhouse", Name = "FK_TB_CONFIG_SOFT")]
[Index("Idconfig", Name = "PK_TB_CONFIG", IsUnique = true)]
public partial class TbConfig
{
    [Key]
    [Column("IDCONFIG")]
    public int Idconfig { get; set; }

    [Column("SINCRONIZARAPI", TypeName = "CHAR(1)")]
    public string? Sincronizarapi { get; set; }

    [Column("GUUID")]
    [StringLength(100)]
    public string? Guuid { get; set; }

    [Column("ORDEMMASCARAANALITICO")]
    public int? Ordemmascaraanalitico { get; set; }

    [Column("ORDEMMASCARAGERENCIAL")]
    public int? Ordemmascaragerencial { get; set; }

    [Column("SOFTHOUSE")]
    public int Softhouse { get; set; }
}
