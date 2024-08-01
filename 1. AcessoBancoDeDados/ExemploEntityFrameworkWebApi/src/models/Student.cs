using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ExemploEntityFrameworkWebApi.src.models;

[Table("student")]
public class Student : _BaseEntityWithId {
  [NotNull]
  [ForeignKey("id_person")]
  public Person Person { get; set; }

  [ForeignKey("id_course")] //para nomeação da tabela intermediaria, somente pelo OnModelCreating
  public ICollection<Course> Courses { get; set; }
}
