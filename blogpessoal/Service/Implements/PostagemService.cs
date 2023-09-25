using blogpessoal.Data;
using blogpessoal.Model;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Service.Implements
{
    public class PostagemService : IPostagemService
    {
        private readonly AppDbContext _context; // campo usado usado para interagir com o banco de dados 
        public PostagemService(AppDbContext context)// context injeção de dependencia 

        {
            _context = context; // isso permite que a classe postagemservice use o contexto banco de dados para realizar operações relacioanados as postagens
        }
            

        
        public Task<Postagem?> Create(Postagem postagem)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Postagem postagem)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Postagem>> GetAll() // quando colocar o async/await permite que o programa continue rodando enquato busca noa banco de dados
        {
            return await _context.Postagens.ToListAsync(); 
        }

        public Task<Postagem?> GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Postagem?> GetByTitulo(string Titulo)
        {
            throw new NotImplementedException();
        }

        public Task<Postagem?> Upadte(Postagem postagem)
        {
            throw new NotImplementedException();
        }
    }
}
