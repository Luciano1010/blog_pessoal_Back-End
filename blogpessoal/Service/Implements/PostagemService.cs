using blogpessoal.Data;
using blogpessoal.Model;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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

        public async Task<IEnumerable<Postagem>> GetAll() // quando colocar o async/await permite que o programa continue rodando enquato busca noa banco de dados
        {
            return await _context.Postagens
                .Include(p => p.Tema)
                .Include(p => p.Usuario)
                .ToListAsync();
        }

        public async Task<Postagem?> GetById(long id) // metodo que vou utilizar pra atualizar os dados por id
        {
            try // depois tratamento de erro caso nao ache nenhuma postagem
            {
                var Postagem = await _context.Postagens
                    .Include(p => p.Tema)
                    .Include(p => p.Usuario)
                    .FirstAsync(i => i.Id == id); // primeiro definir o modo de busca
                return Postagem;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Postagem>> GetByTitulo(string Titulo) // semelhante a consulta like em um banco de dados                          // semelhante ao like do banco de dados
        {
            var Postagem = await _context.Postagens // (local) _context pega todos os livros de postagens e guarda as informações em Postagem
                                 .Include(p => p.Tema)
                                 .Include(p => p.Usuario)
                                 .Where(p => p.Titulo.Contains(Titulo)) // em postagens tem a palavra Titulo? esse metodo faz a procura
                                 .ToListAsync(); // ao achar as postagens que contem Titiulo organize em uma lista e que sera guardada na variavel postagem
            return Postagem; // retorne a lista 
        
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

            postagem.Tema = postagem.Tema is not null ? _context.Temas.FirstOrDefault(t => t.Id == postagem.Tema.Id): null; // essa linha persisti objeto na tabela,ela faz uma busca dentro _context
            postagem.Usuario = postagem.Usuario is not null ? await _context.Users.FirstOrDefaultAsync(u => u.Id == postagem.Usuario.Id) : null;

            await _context.Postagens.AddAsync(postagem); // adicionei na fila , _context representa o banco de dados
            await _context.SaveChangesAsync(); // persisti a informação no banco na coluna posategm

            return postagem; // depois de salvar a funçao no banco ela retornar
        }
        public async  Task<Postagem?> Update(Postagem postagem)
        {
            var PostagemUpdate = await _context.Postagens.FindAsync(postagem.Id);
            
            if(PostagemUpdate is null) // verficando se a informação digitida existe
                return null;
          
            if (postagem.Tema is not null) // verificação se temas existe se nao existir retonar nulo.
            {
                var BuscaTema = await _context.Temas.FindAsync(postagem.Tema.Id);
                if (BuscaTema is null)
                    return null;

                    postagem.Tema = BuscaTema;
            }


            postagem.Tema = postagem.Tema is not null ? _context.Temas.FirstOrDefault(t => t.Id == postagem.Tema.Id) : null;// quandp fazer a atualização persistir os dados
            postagem.Usuario = postagem.Usuario is not null ? await _context.Users.FirstOrDefaultAsync(u => u.Id == postagem.Usuario.Id) : null;
            
            _context.Entry(PostagemUpdate).State = EntityState.Detached; // eu nao quero a informação digitada pra fazer a procura persista
            _context.Entry(postagem).State = EntityState.Modified; // e a informaçao q quero quer persista
            await _context.SaveChangesAsync(); // salvando a alteração

            return postagem; // retorno da atualização
        }
                
            
        public async Task Delete(Postagem postagem)
        {
            _context.Remove(postagem);
            await _context.SaveChangesAsync();
        }



        
       



    }
}
