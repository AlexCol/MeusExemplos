using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ExemploEntityFrameworkWebApi.src.models;

[Table("person")]
public class Person : _BaseEntityWithId {
  //https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties?tabs=data-annotations%2Cwithout-nrt
  [NotNull] //utilizar esse e não required, pois com isso consigo suprimir a obrigatoriedade de vir no json (ver DependenciesBuilder)
  //[MaxLength(100)] //desncessário pois tem no context o tamanho padrão
  [Column("ds_first_name")]
  public string FirstName { get; set; }

  [NotNull] //utilizar esse e não required, pois com isso consigo suprimir a obrigatoriedade de vir no json (ver DependenciesBuilder)
  [Column("ds_last_name")]
  public string LastName { get; set; }

  [Column("ds_document")]
  public string Document { get; set; }

  [ForeignKey("id_gender")] //para realizar deleção em cascata, somente no OnModelCreating no context
  public Gender Gender { get; set; }

  [Column("dt_birth", TypeName = "date")]
  public DateTime DateOfBirth { get; set; }

  [JsonIgnore] //!não buscar o endereço quando se buscar pessoas (evita referencia circular se buscar o endereço)
  public ICollection<Address> Addresses { get; set; }
}
