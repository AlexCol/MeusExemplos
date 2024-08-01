using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ExemploEntityFrameworkWebApi.src.models;

[Table("student")]
public class Student : _BaseEntityWithId {
  [NotNull]
  [ForeignKey("id_person")]
  public Person Person { get; set; }

  //para nomeação da tabela intermediaria, somente pelo OnModelCreating
  //para a nomeação da colune, aqui representa como a outra vai ver essa, por isso pode ser confuso, mas preciso identificar que id_student é como student vai ser reconhecido na relação 
  [ForeignKey("id_student")]
  public ICollection<Course> Courses { get; set; }
}
