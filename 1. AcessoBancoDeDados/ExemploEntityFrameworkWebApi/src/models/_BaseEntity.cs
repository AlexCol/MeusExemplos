using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExemploEntityFrameworkWebApi.src.models;

public abstract class _BaseEntity {
  [Key]
  [Column("id")]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  [Column("dt_edited_at")]
  public DateTime EditedAt { get; set; }

  [Column("dt_created_at")]
  public DateTime CreatedAt { get; set; }
}
