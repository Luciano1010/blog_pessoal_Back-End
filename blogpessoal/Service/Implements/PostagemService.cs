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
            return await _context.Postagens.ToListAsync();
        }

        public async Task<Postagem?> GetById(long id) // metodo que vou utilizar pra atualizar os dados por id
        {
            try // depois tratamento de erro caso nao ache nenhuma postagem
            {
                var Postagem = await _context.Postagens.FirstAsync(i => i.Id == id); // primeiro definir o modo de busca
                return Postagem;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Postagem>> GetByTitulo(string Titulo)                                // semelhante ao like do banco de dados
        {
            var Postagem = await _context.Postagens
                                 .Where(p => p.Titulo.Contains(Titulo))
                                 .ToListAsync();
            return Postagem;
        
        }

        public async Task<Postagem?> Create(Postagem postagem)
        {
            await _context.Postagens.AddAsync(postagem); // adicionei na fila
            await _context.SaveChangesAsync(); // persisti a informação no banco

            return postagem;
        }
        public async  Task<Postagem?> Update(Postagem postagem)
        {
            var PostagemUpdate = await _context.Postagens.FindAsync(postagem.Id);
            
            if(PostagemUpdate is null) // verficando se a informação digitida existe
                return null;
           
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
