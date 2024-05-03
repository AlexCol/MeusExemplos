using System.Reflection;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Exemplo1.src.Attributes.SwaggerExclude;

namespace Exemplo1.src.Extensions.toBuilder;

public static class SwaggerBuilder {
    public static void addSwagger(this WebApplicationBuilder builder) {
        string appName = "Minha API Rest";
        string appVersion = "v1";
        string appDescription = $"{appName} para controle de Autenticação.";
        builder.Services.AddSwaggerGen(c => {
            //!cabeçalho do swagger
            c.SwaggerDoc(appVersion,
            new OpenApiInfo {
                Title = appName, //titulo no swagger                
                Description = appDescription,
                Version = appVersion,
                Contact = new OpenApiContact {
                    Name = "Alexandre",
                    Url = new Uri("http://localhost:5194/")
                }
            });

            //! adiciona cadeado no topo das requisições, pra colocar o token
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("JWT Autenticação usando Bearer no cabeçalho.");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("Para autenticar, informe no campo abaixo: ");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("->> Bearer {Chave JWT}");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("Ex: Bearer eyJhbGciOiJIUzI1N...");

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                Description = stringBuilder.ToString(),
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            //! adiciona cadiadinho em cada requisição, forçando a colocar o token
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }, new List<string>()
                    }
                });
            //! por padrão todos os controllers vão aparecer no swagger, caso se deseje ocultar algum, deve-se colocar o atributo SwaggerExclude 
            //! (pro comportamento oposto, ver trecho comentado no fim do arquivo)
            c.DocInclusionPredicate((docName, apiDesc) => { //! serão analisados todos os metodos dos controladores
                bool classeComBloqueio = false;
                if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

                var controllerType = methodInfo.DeclaringType; //! busca a classe base do metodo
                if (controllerType != null) { //! analisa se controller tem atributo para liberar endpoints
                    classeComBloqueio = controllerType.GetCustomAttributes(true).OfType<SwaggerExcludeAttribute>().Any();
                }

                if (classeComBloqueio) { return false; } //! se a classe tiver o atributo, ignora o atributo individual do metodo

                //! por fim, se não encontrar o atributo na classe, analisa o metodo individualmente
                return !methodInfo.GetCustomAttributes(true).OfType<SwaggerExcludeAttribute>().Any();
            });

            //! necessário pra aparecerem as informações das observações no swagger
            //! no arquivo .csproj adicionar
            //! <GenerateDocumentationFile>true</GenerateDocumentationFile>
            //! <NoWarn>$(NoWarn);1591</NoWarn>
            //! exemplo de anotações, no TesteController
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        });
        builder.Services.AddRouting(opt => opt.LowercaseUrls = true); //!para que fique tudo em minusculo os links no swagger
    }
}

/*
            //! por padrão nenhum controller vai aparecer no swagger, caso se deseje mostrar algum, deve-se colocar o atributo SwaggerInclude
            c.DocInclusionPredicate((docName, apiDesc) => { //! serão analisados todos os metodos dos controladores
                bool classeComParametro = false;
                if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

                var controllerType = methodInfo.DeclaringType; //! busca a classe base do metodo
                if (controllerType != null) { //! analisa se controller tem atributo para liberar endpoints
                    classeComParametro = controllerType.GetCustomAttributes(true).OfType<SwaggerIncludeAttribute>().Any();
                }

                if (classeComParametro) { return true; } //! se a classe tiver o atributo, ignora o atributo individual do metodo

                //! por fim, se não encontrar o atributo na classe, analisa o metodo individualmente
                return methodInfo.GetCustomAttributes(true).OfType<SwaggerIncludeAttribute>().Any();
            });
*/