using blogpessoal.Model;
using blogpessoal.Service.Implements;
using blogpessoal.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using blogpessoal.Service;
using blogpessoal.Model.Security;
using Microsoft.AspNetCore.Authorization;

namespace blogpessoal.Controllers
{

    [Route("~/usuarios")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService; // aqui a interface vai manipular os dados sem necessidade de mexer nos dados da classe
        private readonly IValidator<User> _userValidator; // metodo q avalia se a postagem pode ser feita
        private readonly IAuthService _authService;
        public UserController(IUserService UserService, IValidator<User> UserValidator, IAuthService authService)
        {
            _userService = UserService;
            _userValidator = UserValidator;
            _authService = authService;
        }

        [Authorize]
        [HttpGet("all")]

        public async Task<ActionResult> GetAll()
        {

            return Ok(await _userService.GetAll());

        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetbyId(long id)
        {
            var Resposta = await _userService.GetById(id);

            if (Resposta == null)
                return NotFound();

            return Ok(Resposta);
        }

        [AllowAnonymous]
        [HttpPost("cadastrar")] // criar
       

        public async Task<ActionResult> Create([FromBody] User user) // frombody o objeto postagem no corpo da requisição
        {
            var validarusers = await _userValidator.ValidateAsync(user); // usando o postagem validator vai verificar se o objeto esta dentro dos parametros e sera guardado no validarPostagem         

            if (!validarusers.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validarusers); // que indica que a requisição (ou a validação) não foi bem-sucedida, e ainda fornece o q foi escrito no validarPostagem para fornecer mais detalhes
            }
            var Resposta = await _userService.Create(user);

            if(Resposta is null)
                return BadRequest("Usuario já cadastrado!");

            return CreatedAtAction(nameof(GetbyId), new { id = user.Id }, user);
            // return volta com um valor, createdAtaction manda o codigo de status 200, nameof é pegar um id novo, new cria o objeto junto com o seu id, postagem é o objeto criado com sucesso. 
        }



        [Authorize]
        [HttpPut("atualizar")] // atualizar
        public async Task<ActionResult> Update([FromBody] User user)
        {
            // verificar o id
            if (user.Id == 0) // nao tem id 0 ou numeros negativos
                return BadRequest("Id da postagem é invalido");

            var validaruser = await _userValidator.ValidateAsync(user);
            if (!validaruser.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validaruser);
            }


            // verificar se o email e id 
            var Userupadate = await _userService.GetByUsuario(user.Usuario);
           
            if (Userupadate is not null && Userupadate.Id != user.Id)
                return BadRequest("o Usuario(e-mail) ja esta em uso");
           
            var Resposta = await _userService.Update(user); // aqui atualiza se encontrar o id 

            if (Resposta is null)
                return NotFound("Postagem não Encontrada");

            return Ok(Resposta);

        }

        [AllowAnonymous]
        [HttpPost("logar")] 
        public async Task<ActionResult> Autenticar([FromBody] UserLogin userlogin) // frombody o objeto postagem no corpo da requisição
        {
            var Resposta = await _authService.Autenticar(userlogin);

            if (Resposta is null)
                return Unauthorized("Usuario e/ou Senha são invalidos");

            return Ok(Resposta);
        }




    }

}
