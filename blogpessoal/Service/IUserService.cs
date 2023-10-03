using blogpessoal.Model;

namespace blogpessoal.Service
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll(); // taks trabalha com metodos assincronos, Inumerable trabalha com abstração/ traz todas as postagens

        Task<User?> GetById(long id); // metodo consulta por Id a Users

        Task<User?> GetByUsuario(string usuario); // metodo de busca pelo tipo da Users

        Task<User?> Create(User usuario); // metodo criar nova Users

        Task<User?> Update(User usuario); // metodo de atuaizar

        

    }   
}
