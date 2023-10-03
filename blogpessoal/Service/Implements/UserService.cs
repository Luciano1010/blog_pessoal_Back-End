using blogpessoal.Data;
using blogpessoal.Model;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Service.Implements
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)// context injeção de dependencia 

        {
            _context = context; // isso permite que a classe postagemservice use o contexto banco de dados para realizar operações relacioanados as postagens
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .Include(u => u.Postagem) 
                .ToListAsync();
        }

        public async Task<User?> GetById(long id)
        {
            try 
            {
                var Usuario = await _context.Users
                    .Include(u => u.Postagem)
                    .FirstAsync(i => i.Id == id); 

                Usuario.Senha = ""; // no fronte pra atualizar a senha
                return Usuario;

            }
            catch
            {
                return null;
            }
        }

        public async Task<User?> GetByUsuario(string usuario)
        {
            try // quando usar o first usava o try pra exceptions
            {
                var Buscausuario = await _context.Users
                    .Include(u => u.Postagem)
                    .Where(u => u.Usuario == usuario)
                    .FirstOrDefaultAsync(); // metodo pra retornar um unico objeto da lista

                return Buscausuario;
            }
            catch 
            {
                return null;
            
            }
        }
        public async Task<User?> Create(User usuario)
        {
            var Buscausuario = await GetByUsuario(usuario.Usuario);

            if (Buscausuario is not null) 
            {
                return null;
            }
            if (usuario.Foto is null || usuario.Foto == "")
                usuario.Foto = "https://i.imgur.com/I8MfmC8.png";

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha, workFactor: 10); // metodo de criptografia

            await _context.Users.AddAsync(usuario); 
            await _context.SaveChangesAsync(); 

            return usuario;
        }

        public async Task<User?> Update(User usuario)
        {
            var UsuarioUpdate = await _context.Users.FindAsync(usuario.Id); // findsync é para procurar

            if (UsuarioUpdate is null) //condicional verficando se a informação digitida existe
                return null;

            if (usuario.Foto is null || usuario.Foto == "")
                usuario.Foto = "https://i.imgur.com/I8MfmC8.png";

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha, workFactor: 10);

            _context.Entry(UsuarioUpdate).State = EntityState.Detached; 
            _context.Entry(usuario).State = EntityState.Modified; 
            await _context.SaveChangesAsync(); 

            return usuario; 
        }
    }
}
