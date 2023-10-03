using blogpessoal.Data;
using blogpessoal.Service;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace blogpessoal.Model.Security.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        public AuthService(IUserService userService)

        {
            _userService = userService; 
        }
        public async Task<UserLogin?> Autenticar(UserLogin userLogin)
        {
            string FotoDefault = "https://i.imgur.com/I8MfmC8.png";

            if (userLogin is null || string.IsNullOrEmpty(userLogin.Usuario) || string.IsNullOrEmpty(userLogin.Senha))
                return null;

            var Buscausuario = await _userService.GetByUsuario(userLogin.Usuario);

            if (Buscausuario is not null)
                return null;

            if(!BCrypt.Net.BCrypt.Verify(userLogin.Senha, Buscausuario.Senha))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim (ClaimTypes.Name, userLogin.Usuario)
                }),

                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)


            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);

            userLogin.Id = Buscausuario.Id;
            userLogin.Nome = Buscausuario.Nome;
            userLogin.Foto = Buscausuario.Foto is null? FotoDefault : Buscausuario.Foto;
            userLogin.Token = "Bearer " + tokenHandler.WriteToken(token).ToString(); 
            userLogin.Senha = "";

            return userLogin;



        }
    }
}
