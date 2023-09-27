using blogpessoal.Data;
using blogpessoal.Model;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Service.Implements
{
    public class TemasService : ItemaService
    {
        private readonly AppDbContext _context;
        public TemasService(AppDbContext context)// context injeção de dependencia 

        {
            _context = context; // isso permite que a classe postagemservice use o contexto banco de dados para realizar operações relacioanados as postagens
        }
        
        public async Task<IEnumerable<Tema>> GetAll()
        {
            return await _context.Temas
                .Include(t => t.Postagem) // include ele vai trazer o tema e as postagem correspondentes
                .ToListAsync();
        }
        public async Task<Tema?> Create(Tema temas)
        {
            await _context.Temas.AddAsync(temas); // adicionei na fila , _context representa o banco de dados
            await _context.SaveChangesAsync(); // persisti a informação no banco na coluna posategm

            return temas;
        }
        public async Task<Tema?> GetById(long id)
        {
            try // depois tratamento de erro caso nao ache nenhuma postagem
            {
                var Tema = await _context.Temas
                    .Include(t => t.Postagem)
                    .FirstAsync(i => i.Id == id); // primeiro definir o modo de busca
                return Tema;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Tema>> GetByDescricao(string descricao)
        {
            var Tema = await _context.Temas // (local) _context pega todos os livros de postagens e guarda as informações em Postagem
                                   .Include(t => t.Postagem)
                                   .Where(t => t.Descricao.Contains(descricao)) // pro end point temas/descricao/atualização - colocar a palavra q identifica atualização
                                   .ToListAsync(); // ao achar as postagens que contem Titiulo organize em uma lista e que sera guardada na variavel postagem
            return Tema; // retorne a lista 
        }
        public async Task<Tema?> Update(Tema temas)
        {
            var TemaUpdate = await _context.Temas.FindAsync(temas.Id);

            if (TemaUpdate is null) // verficando se a informação digitida existe
                return null;

            _context.Entry(TemaUpdate).State = EntityState.Detached; // eu nao quero a informação digitada pra fazer a procura persista
            _context.Entry(temas).State = EntityState.Modified; // e a informaçao q quero quer persista
            await _context.SaveChangesAsync(); // salvando a alteração

            return temas; // retorno da atualização
        }
        public async Task Delete(Tema temas)
        {
            _context.Remove(temas);
            await _context.SaveChangesAsync();
        }
       




    }
}
