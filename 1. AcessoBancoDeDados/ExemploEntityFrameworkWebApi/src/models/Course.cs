using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ExemploEntityFrameworkWebApi.src.models;

[Table("course")]
public class Course : _BaseEntityWithId {
  [NotNull]
  [Column("ds_name")]
  public string Name { get; set; }

  [NotNull]
  [Column("sn_ativo")]
  public bool Ativo { get; set; } = true;

  [ForeignKey("id_student")] //para nomeação da tabela intermediaria, somente pelo OnModelCreating
  public ICollection<Student> Students { get; set; }
}