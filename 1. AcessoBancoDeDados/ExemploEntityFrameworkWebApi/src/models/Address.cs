using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExemploEntityFrameworkWebApi.src.models;

[Table("address")]
public class Address : _BaseEntityWithId {
  [MaxLength(200)]
  [Required]
  [Column("ds_street")]
  public string Street { get; set; }

  [Required]
  [Column("ds_city")]
  public string City { get; set; }

  [Required]
  [Column("ds_state")]
  public string State { get; set; }

  [Required]
  [Column("ds_number")]
  public string Number { get; set; }

  [Required]
  [Column("ds_zip_code")]
  public string ZipCode { get; set; }

  [Required]
  [ForeignKey("id_person")]
  public Person Person { get; set; }
}
