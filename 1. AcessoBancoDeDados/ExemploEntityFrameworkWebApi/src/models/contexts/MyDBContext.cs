using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExemploEntityFrameworkWebApi.src.models.contexts;

public class MyDBContext : DbContext {
  public DbSet<Person> Persons { get; set; }
  public DbSet<Address> Addresses { get; set; }
  public DbSet<Gender> Genders { get; set; }

  public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }

  //!aqui ficam as configurações 'default' dos campos
  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) {
    configurationBuilder.Properties<string>()
        .HaveMaxLength(100); //com isso digo que todos os campos do tipo string terão setados tamanho max de 100 se não for informado o contrário
  }

  //! duas configuracoes abaixo para que as datas de alteração e criação sejam preenchidas ao salvar assincrono e sincrono
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

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
    var entries = ChangeTracker.Entries<_BaseEntity>()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

    foreach (var entry in entries) {
      if (entry.State == EntityState.Added) {
        ((_BaseEntity)entry.Entity).CreatedAt = DateTime.Now;
        ((_BaseEntity)entry.Entity).EditedAt = DateTime.Now;
      } else if (entry.State == EntityState.Modified) {
        entry.Property(nameof(_BaseEntity.CreatedAt)).IsModified = false; //não muda a data de criação
        ((_BaseEntity)entry.Entity).EditedAt = DateTime.Now;

      }
    }

    return await base.SaveChangesAsync(cancellationToken);
  }

}
