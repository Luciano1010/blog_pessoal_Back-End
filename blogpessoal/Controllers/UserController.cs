using blogpessoal.Model;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using blogpessoal.Service;
using blogpessoal.Model.Security;
using Microsoft.AspNetCore.Authorization;

namespace blogpessoal.Controllers
{
    [Authorize]
    [Route("~/usuarios")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService; 
        private readonly IValidator<User> _userValidator; 
        private readonly IAuthService _authService;
        public UserController(IUserService UserService, IValidator<User> UserValidator, IAuthService AuthService)
        {
            _userService = UserService;
            _userValidator = UserValidator;
            _authService = AuthService;
        }

       
        [HttpGet("all")]

        public async Task<ActionResult> GetAll()
        {

            return Ok(await _userService.GetAll());

        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult> GetbyId(long id)
        {
            var Resposta = await _userService.GetById(id);

            if (Resposta == null)
                return NotFound();

            return Ok(Resposta);
        }

       

        [AllowAnonymous]
        [HttpPost("cadastrar")] 
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
           
        }



        
        [HttpPut("atualizar")] 
        public async Task<ActionResult> Update([FromBody] User user)
        {
            
            if (user.Id == 0) 
                return BadRequest("Id da postagem é invalido");

            var validaruser = await _userValidator.ValidateAsync(user);
            if (!validaruser.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validaruser);
            }


            
            var Userupadate = await _userService.GetByUsuario(user.Usuario);
           
            if (Userupadate is not null && Userupadate.Id != user.Id)
                return BadRequest("o Usuario(e-mail) ja esta em uso");
           
            var Resposta = await _userService.Update(user); 

            if (Resposta is null)
                return NotFound("Postagem não Encontrada");

            return Ok(Resposta);

        }

        [AllowAnonymous]
        [HttpPost("logar")] 
        public async Task<ActionResult> Autenticar([FromBody] UserLogin userlogin) 
        {
            var Resposta = await _authService.Autenticar(userlogin);

            if (Resposta is null)
                return Unauthorized("Usuario e/ou Senha são invalidos");

            return Ok(Resposta);
        }




    }

}
