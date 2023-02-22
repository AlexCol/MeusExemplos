using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[AllowAnonymous]
[Route("api/token")]
public class CriaTokenAutenticacao : ControllerBase
{
    private readonly IConfiguration configuration;
    private readonly ILogger<LoginViewModel> log;

    public CriaTokenAutenticacao(IConfiguration _configuration, ILogger<LoginViewModel> _log)
    {
        configuration = _configuration;
        log = _log;
    }


    [HttpPost("login")]
    public IActionResult Login(
        //[FromServices] IConfiguration configuration, //!movido para o construtor
        //[FromServices] ILogger<LoginViewModel> log, //!movido para o construtor
        [FromBody] LoginViewModel model
    )
    {
        if (model.Username == "usuario" && model.Password == "senha")
        {
            log.LogWarning("Autenticando");

            var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);

            var subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Email, model.Username),
                    new Claim(ClaimTypes.NameIdentifier, model.Username),
                });

            System.Console.WriteLine(DateTime.UtcNow.AddSeconds(1));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Audience = configuration["JwtBearerTokenSettings:Audience"],
                Issuer = configuration["JwtBearerTokenSettings:Issuer"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddSeconds(int.Parse(configuration["JwtBearerTokenSettings:ExpiryTimeInSeconds"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new { token = tokenString });
        }
        return Unauthorized();
    }
}