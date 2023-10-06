using blogpessoal.Model;
using blogpessoal.Service.Implements;
using blogpessoal.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults; 
using Microsoft.AspNetCore.Mvc;
using blogpessoal.Service;
using Microsoft.AspNetCore.Authorization;

namespace blogpessoal.Controllers
{
    [Authorize]
    [Route("~/temas")]
    [ApiController]
    public class TemaController : ControllerBase
    {
        private readonly ItemaService _temaService; // aqui a interface vai manipular os dados sem necessidade de mexer nos dados da classe
        private readonly IValidator<Tema> _temaValidator; // metodo q avalia se a postagem pode ser feita
            
        public TemaController(ItemaService TemaService, IValidator<Tema> TemaValidator)
        {
            _temaService = TemaService;
            _temaValidator = TemaValidator;
        }

        [HttpGet] 
       
        public async Task<ActionResult> GetAll() 
        {

            return Ok(await _temaService.GetAll());
        
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetbyId(long id)
        {
            var Resposta = await _temaService.GetById(id);

            if (Resposta == null)
                return NotFound();

            return Ok(Resposta);
        }


        [HttpGet("descricao/{descricao}")]
        public async Task<ActionResult> GetByDescricao(string Descricao)

        {
            return Ok(await _temaService.GetByDescricao(Descricao));
        }

       
        
        [HttpPost] // criar

        public async Task<ActionResult> Create([FromBody] Tema temas) // frombody o objeto postagem no corpo da requisição
        {
            var validartemas = await _temaValidator.ValidateAsync(temas); // usando o postagem validator vai verificar se o objeto esta dentro dos parametros e sera guardado no validarPostagem         

            if (!validartemas.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validartemas); // que indica que a requisição (ou a validação) não foi bem-sucedida, e ainda fornece o q foi escrito no validarPostagem para fornecer mais detalhes
            }
            await _temaService.Create(temas);

            return CreatedAtAction(nameof(GetbyId), new { id = temas.Id}, temas);
            // return volta com um valor, createdAtaction manda o codigo de status 200, nameof é pegar um id novo, new cria o objeto junto com o seu id, postagem é o objeto criado com sucesso. 
        }


        [HttpPut] // atualizar
        public async Task<ActionResult> Update([FromBody] Tema temas)
        {
            if (temas.Id == 0) // nao tem id 0 ou numeros negativos
                return BadRequest("Id da postagem é invalido");

            var validartemas = await _temaValidator.ValidateAsync(temas);
            if (!validartemas.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validartemas);
            }

            var Resposta = await _temaService.Update(temas); // aqui atualiza se encontrar o id 

            if (Resposta is null)
                return NotFound("Postagem não Encontrada");

            return Ok(Resposta);

        }
        [HttpDelete("{id}")] // delete 

        public async Task<IActionResult> Delete(long id)
        {
            var BuscaPostagem = await _temaService.GetById(id);


            if (BuscaPostagem is null)
                return NotFound("Postagem não encontrada");

            await _temaService.Delete(BuscaPostagem);
            return NoContent();


        }


    }
}
