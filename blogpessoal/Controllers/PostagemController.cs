using blogpessoal.Model;
using blogpessoal.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogpessoal.Controllers
{
    // classe que verifica os resultados
    [Authorize]
    [Route("~/postagens")] // codigo que define um lugar especifico exemplo https://exemplo.com/postagens

    [ApiController] // é um serviço que permite diferentes programas(aplicativos,moveis e sites) se couminiquem com meu aplicativo
    public class PostagemController : ControllerBase
    {

        private readonly IPostagemService _postagemService; // aqui a interface vai manipular os dados sem necessidade de mexer nos dados da classe
        private readonly IValidator<Postagem> _postagemValidator; // metodo q avalia se a postagem pode ser feita

        public PostagemController(IPostagemService postagemService, IValidator<Postagem> postagemValidator)

        {
            _postagemService = postagemService;
            _postagemValidator = postagemValidator;
        }

        [HttpGet] // 1 end point os demais nao podem ser igual HTTPGET
        // quando colocar o async tenho q colocar o await
        public async Task<ActionResult> GetAll()  // get all pede a _postagemService para obter uma lista de informações relacionados as postagens

        {
            return Ok(await _postagemService.GetAll()); // await permite aplicação rodar quando esta sendo feito a busca
        }

        [HttpGet("{id}")] // quanso sempre colocamos "{}" é uma variavel que determina o parametro da variavvel
        public async Task<ActionResult> GetbyId(long id)
        {
            var Resposta = await _postagemService.GetById(id);

            if (Resposta == null)
                return NotFound();

            return Ok(Resposta);

        }

        [HttpGet("titulo/{Titulo}")]
        public async Task<ActionResult> GetByTitulo(string Titulo)

        {
            return Ok(await _postagemService.GetByTitulo(Titulo));
        }

        [HttpPost] // criar

        public async Task<ActionResult> Create([FromBody] Postagem postagem) // frombody o objeto postagem no corpo da requisição
        {
            var validarPostagem = await _postagemValidator.ValidateAsync(postagem); // usando o postagem validator vai verificar se o objeto esta dentro dos parametros e sera guardado no validarPostagem         

            if (!validarPostagem.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validarPostagem); // que indica que a requisição (ou a validação) não foi bem-sucedida, e ainda fornece o q foi escrito no validarPostagem para fornecer mais detalhes
            }
           var Resposta = await _postagemService.Create(postagem); // so tema que nao existe, tratamento de erro

            if (Resposta is null) // um condicional
                return BadRequest("Tema não encontrado"); // se for nulo retorne essa msg

            return CreatedAtAction(nameof(GetbyId), new { id = postagem.Id }, postagem);
           // return volta com um valor, createdAtaction manda o codigo de status 200, nameof é pegar um id novo, new cria o objeto junto com o seu id, postagem é o objeto criado com sucesso. 
        }

        [HttpPut] // atualizar
        public async Task<ActionResult> Update([FromBody] Postagem postagem) 
        {
            if (postagem.Id == 0) // nao tem id 0 ou numeros negativos
                return BadRequest("Id da postagem é invalido");
           
            var validarPostagem = await _postagemValidator.ValidateAsync(postagem);
            if (!validarPostagem.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validarPostagem);
            }

            var Resposta = await _postagemService.Update(postagem); // aqui atualiza se encontrar o id 

            if (Resposta is null)
                return NotFound("Postagem e/ ou Tema Encontrada");

            return Ok(Resposta);    

        }

        [HttpDelete("{id}")] // delete 

        public async Task<IActionResult> Delete(long id) 
        {
            var BuscaPostagem = await _postagemService.GetById(id);


            if (BuscaPostagem is null)
                return NotFound("Postagem não encontrada");

            await _postagemService.Delete(BuscaPostagem);
            return NoContent();
                
   
        }


    }

}
    
        
        



        
    
    
