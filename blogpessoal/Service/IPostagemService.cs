using blogpessoal.Model;

namespace blogpessoal.Service
{
    public interface IPostagemService
    {
        Task<IEnumerable<Postagem>> GetAll(); // taks trabalha com metodos assincronos, Inumerable trabalha com abstração/ traz todas as postagens

        Task<Postagem?> GetById(long id); // metodo consulta por Id a postagem

        Task<IEnumerable<Postagem>> GetByTitulo(string Titulo); // metodo de busca pelo tipo da postagem
    
        Task<Postagem?> Create(Postagem postagem); // metodo criar nova postagem

        Task<Postagem?> Update(Postagem postagem); // metodo de atuaizar

        Task Delete(Postagem postagem); // metodo deletar
    }// taks é pra trabalhar com programação assincrona

    
}


