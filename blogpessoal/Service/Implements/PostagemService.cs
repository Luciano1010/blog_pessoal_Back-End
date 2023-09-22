using blogpessoal.Data;
using blogpessoal.Model;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Service.Implements
{
    public class PostagemService : IPostagemService
    {
        private readonly AppDbContext _context; // injeção de dependecia, quem vai criar os objetos sera o AspNET.
        public PostagemService(AppDbContext context) 

        {
            _context = context;
        }
            

        
        public Task<Postagem?> Create(Postagem postagem)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Postagem postagem)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Postagem>> GetAll() // quando colocar o async tenho q colocar o await
        {
            return await _context.Postagens.ToListAsync(); // ele vai trazer todas as informções que solicitar
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
