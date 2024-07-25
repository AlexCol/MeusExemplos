using Microsoft.EntityFrameworkCore;

namespace ExemploEntityFrameworkWebApi.src.models.contexts;

public class MySqlContext : DbContext {
  public DbSet<Person> Persons { get; set; }
  public DbSet<Address> Addresses { get; set; }
  public DbSet<Gender> Genders { get; set; }

  public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) { }

  //!aqui ficam as configurações 'default' dos campos
  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) {
    configurationBuilder.Properties<string>()
        .HaveMaxLength(100); //com isso digo que todos os campos do tipo string terão setados tamanho max de 100 se não for informado o contrário
  }

  public override int SaveChanges() {
    var entries = ChangeTracker.Entries<_BaseEntity>()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

    foreach (var entry in entries) {
      if (entry.State == EntityState.Added) { //!seta valores padroes ao inserir um novo registro
        ((_BaseEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
        ((_BaseEntity)entry.Entity).EditedAt = DateTime.UtcNow;
      } else if (entry.State == EntityState.Modified) { //! valores automativos ao ser realizado update do registro
        ((_BaseEntity)entry.Entity).EditedAt = DateTime.UtcNow;
        entry.Property("EditedAt").IsModified = false;
      }
    }

    return base.SaveChanges();
  }

}
