
using blogpessoal.Model;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Data
{
    // classe do banco de dados
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Postagem>().ToTable("tb_postagens"); // metodo resposavel para criar as tabelas dentro do banco
            modelBuilder.Entity<Tema>().ToTable("tb_temas"); // tabela temas
            modelBuilder.Entity<User>().ToTable("tb_usuarios");

            _ = modelBuilder.Entity<Postagem>() // metodo de relacionamento entre clases
                .HasOne(_ => _.Tema) // tema lado 1
                .WithMany(t => t.Postagem) // lado 2, tema ira se relacionar com postagens
                .HasForeignKey("TemaId")
                .OnDelete(DeleteBehavior.Cascade);// quando apagar o tema - ira apagar tbm as postagens relacionadas ao tema deletado
           
            _ = modelBuilder.Entity<Postagem>() // metodo de relacionamento entre clases
                .HasOne(_ => _.Usuario) // tema lado 1
                .WithMany(u => u.Postagem) // lado 2, tema ira se relacionar com postagens
                .HasForeignKey("UsuarioId") // nome da chave estrangeira.
      
                .OnDelete(DeleteBehavior.Cascade);// quando apagar o tema - ira apagar tbm as postagens relacionadas ao tema deletado
        }

        public DbSet<Postagem> Postagens { get; set; } = null!; // registrar  Dbset - Obejto responsavel por manipular a Tabela
        public DbSet<Tema> Temas { get; set; } = null!; // objeto responsavel por manipular os temas, sendo usado na posatagem service no Getall.
        public DbSet<User> Users { get; set; } = null!;
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) // ele salva os dados 
        {
            var insertedEntries = this.ChangeTracker.Entries()
                                   .Where(x => x.State == EntityState.Added)
                                   .Select(x => x.Entity);

            foreach (var insertedEntry in insertedEntries)
            {
                //Se uma propriedade da Classe Auditable estiver sendo criada. 
                if (insertedEntry is Auditable auditableEntity)
                {
                    auditableEntity.Data = new DateTimeOffset(DateTime.Now, new TimeSpan(-3, 0, 0));
                }
            }

            var modifiedEntries = ChangeTracker.Entries()
                       .Where(x => x.State == EntityState.Modified)
                       .Select(x => x.Entity);

            foreach (var modifiedEntry in modifiedEntries) // procurar todos os objetos persistidos ao mesmo tempo 
            {
                //Se uma propriedade da Classe Auditable estiver sendo atualizada.  
                if (modifiedEntry is Auditable auditableEntity)
                {
                    auditableEntity.Data = new DateTimeOffset(DateTime.Now, new TimeSpan(-3, 0, 0)); // ele atualiza a data da postagem
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


    }



}

