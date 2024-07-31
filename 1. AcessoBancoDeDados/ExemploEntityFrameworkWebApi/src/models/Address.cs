using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ExemploEntityFrameworkWebApi.src.models;

[Table("address")]
public class Address : _BaseEntityWithId {
  //https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties?tabs=data-annotations%2Cwithout-nrt
  [MaxLength(200)]
  [NotNull]
  [Column("ds_street")]
  public string Street { get; set; }

  [NotNull]
  [Column("ds_city")]
  public string City { get; set; }

  [NotNull]
  [Column("ds_state")]
  public string State { get; set; }

  [NotNull]
  [Column("ds_number")]
  public string Number { get; set; }

  [NotNull]
  [Column("ds_zip_code")]
  public string ZipCode { get; set; }

  [NotNull]
  [ForeignKey("id_person")]
  public Person Person { get; set; }
}
