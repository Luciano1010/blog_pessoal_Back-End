using blogpessoal.Model;
using blogpessoal.Service;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace blogpessoal.Controllers
{
    [Route("~/postagens")] // codigo que define um lugar especifico exemplo https://exemplo.com/postagens

    [ApiController] // é um serviço que permite diferentes programas(aplicativos,moveis e sites) se couminiquem com meu aplicativo
    public class PostagemController : ControllerBase
    {
            
            private readonly IPostagemService _postagemService; // metodo de criação
            private readonly IValidator<Postagem> _postagemValidator; // metodo q avalia se a postagem pode ser feita
       
        public  PostagemController(IPostagemService postagemService, IValidator<Postagem> postagemValidator)

        {
            _postagemService = postagemService;
            _postagemValidator = postagemValidator;
        }

        [HttpGet] // end point
        // quando colocar o async tenho q colocar o await
        public async Task<ActionResult> GetAll()  // get all pede a _postagemService para obter uma lista de informações relacionados as postagens
             
        {
            return Ok(await _postagemService.GetAll()); // await permite aplicação rodar quando esta sendo feito a busca
        }
            
    } 

}
    
        
        



        
    
    
