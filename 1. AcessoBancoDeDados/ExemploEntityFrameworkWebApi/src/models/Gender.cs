using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExemploEntityFrameworkWebApi.src.models;

[Table("gender")]
public class Gender : _BaseEntity {
  [Column("ds_description")]
  public string Description { get; set; }
}
