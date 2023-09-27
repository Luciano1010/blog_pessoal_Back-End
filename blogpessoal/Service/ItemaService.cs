using blogpessoal.Model;

namespace blogpessoal.Service
{
    public interface ItemaService
    {
        Task<IEnumerable<Tema>> GetAll(); // taks trabalha com metodos assincronos, Inumerable trabalha com abstração/ traz todas as postagens

        Task<Tema?> GetById(long id); // metodo consulta por Id a Temas

        Task<IEnumerable<Tema>> GetByDescricao(string Descricao); // metodo de busca pelo tipo da Temas

        Task<Tema?> Create(Tema temas); // metodo criar nova Temas

        Task<Tema?> Update(Tema temas); // metodo de atuaizar

        Task Delete(Tema temas); // metodo deletar

    }   
}
