using System;
using Controllers.Requests;
using Controllers.Responses;
using Dominio;
using Dominio.Classes;
using Dominio.Classes.Extensoes;
using Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository;

namespace ProjetoLOFT2.Controllers;

[ApiController]
[Route("api/personagem")]
public class PersonagemControler : ControllerBase
{
    private readonly ILogger<PersonagemControler> _logger;

    public PersonagemControler(ILogger<PersonagemControler> logger)
    {
        _logger = logger;
    }

    [HttpGet("{id}")]
    public IResult Get(int id)
    {
        _logger.LogInformation("Buscando personagem.");
        PersonagemResponse? personagem = PersonagensRepository.buscaPorId(id);

        if (personagem == null)
        {
            _logger.LogError("Erro na busca.");
            return Results.BadRequest("Personagem nao existe.");
        }

        return Results.Ok(personagem);
    }

    [HttpGet]
    public IResult Get()
    {
        return Results.Ok(PersonagensRepository.listaPersonagens());
    }

    [HttpPost]
    public IResult Post([FromBody] PersonagemRequest personagemRequest)
    {
        try
        {
            BaseClass novaClasse = personagemRequest.classe.retornaClasse();
            int novoId = PersonagensRepository.novoId();
            Personagem novoPersonagem = new Personagem(novoId, personagemRequest.nome, novaClasse);

            if (!novoPersonagem.IsValid)
            {
                var errors = novoPersonagem.Notifications.ConverteParaProblemDetails();
                return Results.ValidationProblem(errors);
            }

            PersonagensRepository.Add(novoPersonagem);

            return Results.Created($"Personagem criado. Id: {novoId}", novoId);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }
}
