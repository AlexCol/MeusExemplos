using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExemploEntityFrameworkWebApi.src.models;

[Table("person")]
public class Person : _BaseEntity {
  [Required]
  //[MaxLength(100)] //desncessário pois tem no context o tamanho padrão
  [Column("ds_first_name")]
  public string First_Name { get; set; }

  [Required]
  [Column("ds_last_name")]
  public string Last_Name { get; set; }

  [Column("ds_document")]
  public string Document { get; set; }

  [ForeignKey("id_gender")]
  public Gender Gender { get; set; }

  [Column("dt_birth", TypeName = "date")]
  public DateTime DateOfBirth { get; set; }

  public ICollection<Address> Addresses { get; set; }
}
