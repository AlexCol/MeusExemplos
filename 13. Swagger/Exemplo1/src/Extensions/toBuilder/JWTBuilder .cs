using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exemplo1.src.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Exemplo1.src.Extensions.toBuilder;


public static class JWTBuilder {
    //! Método para configurar o serviço JWT
    public static void addJWTService(this WebApplicationBuilder builder) {
        //! Cria uma instância de TokenModel para armazenar as configurações do token
        var tokenModel = new TokenModel();

        //! Configura o tokenModel a partir das configurações fornecidas no arquivo de configuração
        new ConfigureFromConfigurationOptions<TokenModel>(
                builder.Configuration.GetSection("TokenConfiguration")
        ).Configure(tokenModel);

        //! Registra o tokenModel como um serviço singleton
        builder.Services.AddSingleton(tokenModel);

        //! Configura a autenticação para usar JWT
        builder.Services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => {
            //! Configura os parâmetros de validação do token JWT
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = tokenModel.Issuer,
                ValidAudience = tokenModel.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenModel.Secret)),
                ClockSkew = TimeSpan.Zero //! Tempo de tolerância após o tempo de expiração do token
            };
        });

        //! Configura as políticas de autorização
        builder.Services.AddAuthorization(auth => {
            //! Define uma política padrão que exige autenticação JWT
            auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                .RequireAuthenticatedUser()
                                .Build();
        });
    }
}

