
using blogpessoal.Model;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Data
{
    // classe do banco de dados
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Postagem>().ToTable("tb_postagens"); // metodo resposavel para criar as tabelas dentro do banco
        }

      
       
        public DbSet<Postagem> Postagens { get; set; } = null!; // registrar  Dbset - Obejto responsavel por manipular a Tabela

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
                    auditableEntity.Data = DateTimeOffset.UtcNow;
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
                    auditableEntity.Data = DateTimeOffset.UtcNow; // ele atualiza a data da postagem
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


    }
}
