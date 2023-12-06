using blogpessoal.Data;
using blogpessoal.Model;
using Microsoft.EntityFrameworkCore;


namespace blogpessoal.Service.Implements
{
    // postagem service faz a regras de negocios
    // a interface faz o desacoplamento das classes
    public class PostagemService : IPostagemService
    {
        private readonly AppDbContext _context; // campo usado usado para interagir com o banco de dados somente leitura
        public PostagemService(AppDbContext context)// context injeção de dependencia 

        {
            _context = context; // isso permite que a classe postagemservice use o contexto banco de dados para realizar operações relacioanados as postagens
        }

        public async Task<IEnumerable<Postagem>> GetAll()
        {
          return await _context.Postagens.Include(p => p.Tema).Include(p => p.Usuario).ToListAsync();
      }

        public async Task<Postagem?> GetById(long id) // metodo que vou utilizar pra atualizar os dados por id
        {
          try {
                var Postagem = await _context.Postagens.Include(p => p.Tema).Include(p => p.Usuario).FirstAsync(i => i.Id == id);
                return Postagem;
            }
            catch {
                return null;
            }
        }

        public async Task<IEnumerable<Postagem>> GetByTitulo(string titulo)
            {
                var Postagem = await _context.Postagens
                                .Include(p => p.Tema)
                                .Include(p => p.Usuario)
                                .Where(p => p.Titulo.Contains(titulo))
                                .ToListAsync();

            return Postagem;
            }

        public async Task<Postagem?> Create(Postagem postagem) //  cria uma nova postagem
        {
            if(postagem.Tema is not null)
            {
                var BuscaTema = await _context.Temas.FindAsync(postagem.Tema.Id);
                if (BuscaTema is null)
                    return null;

                    postagem.Tema = BuscaTema;
            }

            
            postagem.Usuario = postagem.Usuario is not null ? await _context.Users.FirstOrDefaultAsync(u => u.Id == postagem.Usuario.Id) : null;

            await _context.Postagens.AddAsync(postagem); 
            await _context.SaveChangesAsync(); 

            return postagem; 
        }
        public async  Task<Postagem?> Update(Postagem postagem)
        {
            var PostagemUpdate = await _context.Postagens.FindAsync(postagem.Id);
            
            if(PostagemUpdate is null) 
                return null;
          
            if (postagem.Tema is not null) 
            {
                var BuscaTema = await _context.Temas.FindAsync(postagem.Tema.Id);
                if (BuscaTema is null)
                    return null;

                    postagem.Tema = BuscaTema;
            }


         
            postagem.Usuario = postagem.Usuario is not null ? await _context.Users.FirstOrDefaultAsync(u => u.Id == postagem.Usuario.Id) : null;            
            _context.Entry(PostagemUpdate).State = EntityState.Detached; 
            _context.Entry(postagem).State = EntityState.Modified; 
            await _context.SaveChangesAsync(); 
            return postagem; 
        }
                
            
        public async Task Delete(Postagem postagem)
        {
            _context.Remove(postagem);
            await _context.SaveChangesAsync();
        }

    }
}
