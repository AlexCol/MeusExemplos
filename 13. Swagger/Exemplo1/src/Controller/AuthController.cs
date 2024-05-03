using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Exemplo1.src.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Exemplo1.src.Controller;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AuthController : ControllerBase {

    TokenModel _configuration;
    public AuthController(TokenModel configuration) {
        _configuration = configuration;
    }

    //! caso se deseje adicionar descrições ao endpoint
    /// <summary>
    /// Realiza login para acesso.
    /// </summary>
    /// 
    /// <returns>JWT token de acesso.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Todo
    ///     {
    ///        "usuario": "loginDeAcesso",
    ///        "senha": "suaSenha"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="400">If the item is null</response>    
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginModel model) {
        if (model.Usuario == "usuario" && model.Senha == "senha") {
            var key = Encoding.ASCII.GetBytes(_configuration.Secret);

            var subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Email, "eu_axil@yahoo.com.br"),
                    new Claim(ClaimTypes.NameIdentifier, "UserName"),
                    new Claim("MinhaVariavel2", "teste2")
                });

            var meuClaim = new Claim(
                "MinhaVariavel", "variavel"
            );
            subject.AddClaim(meuClaim);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = subject,
                Audience = _configuration.Audience,
                Issuer = _configuration.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddDays(_configuration.DaysToExpire)
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new { token = tokenString });
        }
        return Unauthorized();
    }
}
