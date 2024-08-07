using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ExemploEntityFrameworkWebApi.src.models;

[Table("course")]
public class Course : _BaseEntityWithId {
  [NotNull]
  [Column("ds_name")]
  public string Name { get; set; }

  [NotNull]
  [Column("sn_ativo")]
  public bool Ativo { get; set; } = true;

  //para nomeação da tabela intermediaria, somente pelo OnModelCreating
  //para a nomeação da colune, aqui representa como a outra vai ver essa, por isso pode ser confuso, mas preciso identificar que id_student é como student vai ser reconhecido na relação 
  [ForeignKey("id_course")]
  [JsonIgnore] //!não buscar o endereço quando se buscar pessoas (evita referencia circular se buscar o endereço)
  public ICollection<Student> Students { get; set; }
}