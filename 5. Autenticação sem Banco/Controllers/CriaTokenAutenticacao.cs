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
    public async Task<IActionResult> Login(
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
                    new Claim("MinhaVariavel2", "teste2")
                });

            var meuClaim = new Claim(
                "MinhaVariavel", "variavel"
            );
            subject.AddClaim(meuClaim);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Audience = configuration["JwtBearerTokenSettings:Audience"],
                Issuer = configuration["JwtBearerTokenSettings:Issuer"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddSeconds(int.Parse(configuration["JwtBearerTokenSettings:ExpiryTimeInSeconds"]))
            };

            //mantem o metodo sincorono (com declaração do metodo como "public IActionResult Login(")
            // var tokenHandler = new JwtSecurityTokenHandler();
            // var token = tokenHandler.CreateToken(tokenDescriptor);
            // var tokenString = tokenHandler.WriteToken(token);
            // return Ok(new { token = tokenString });

            //torna ele assincrono (com declaração do metodo como "public async Task<IActionResult> Login("
            var tokenHandler = new JwtSecurityTokenHandler();
            var unsignedToken = await Task.FromResult(tokenHandler.CreateJwtSecurityToken(tokenDescriptor)); // Adicionando await aqui
            var signingCredentials = tokenDescriptor.SigningCredentials;
            var signingKey = signingCredentials.Key;
            var signingAlgorithm = signingCredentials.Algorithm;
            var signedToken = new JwtSecurityTokenHandler().WriteToken(unsignedToken);

            return Ok(new { token = signedToken });
        }
        return Unauthorized();
    }
}