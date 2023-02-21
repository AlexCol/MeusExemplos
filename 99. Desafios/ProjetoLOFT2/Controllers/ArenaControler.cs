using System;
using System.Collections.Generic;
using Controllers.Responses;
using Controllers.Responses.Extensions;
using Dominio;
using Dominio.Arena;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository;

namespace Controllers;

[ApiController]
[Route("api/arena")]
public class ArenaControler : ControllerBase
{
    private readonly ILogger<ArenaControler> _logger;

    public ArenaControler(ILogger<ArenaControler> logger)
    {
        _logger = logger;
    }

    //!metodo 1, passando os parametros no caminho, cada um separado por barra
    [HttpPut("{idPersonagem1}/{idPersonagem2}")]
    public IResult Put(int idPersonagem1, int idPersonagem2)
    {
        var p1 = idPersonagem1;
        var p2 = idPersonagem2;
        return RealizaCombate(p1, p2);
    }

    //!metodo 2, passando os parametros por query string da URL
    [HttpPut]
    public IResult Put([FromQuery] string? idP1, [FromQuery] string? idP2) //mudado pra string pra não dar conflito com a que já existe
    {
        try
        {
            int p1;
            int p2;
            int.TryParse(idP1, out p1);
            int.TryParse(idP2, out p2);

            return RealizaCombate(p1, p2);
        }
        catch (Exception e)
        {
            return Results.BadRequest($"Parametros informados inválidos. {e.Message}");
        }
    }

    private IResult RealizaCombate(int p1, int p2)
    {
        if (p1 == p2)
            return Results.BadRequest("Um personagem não pode lutar contra ele mesmo.");

        string? validaPersonagem;
        PersonagemResponse? playerResponse1 = PersonagensRepository.buscaPorId((int)p1);
        validaPersonagem = PersonagemValido(playerResponse1, 1);
        if (validaPersonagem != null)
            return Results.BadRequest(validaPersonagem);

        PersonagemResponse? playerResponse2 = PersonagensRepository.buscaPorId((int)p2);
        validaPersonagem = PersonagemValido(playerResponse2, 2);
        if (validaPersonagem != null)
            return Results.BadRequest(validaPersonagem);

#pragma warning disable CS8604  //?desabilitando aqui o warning, pq o compilador não identifica que tratei na PersonagemValido pra não ser null
        Personagem playe1 = playerResponse1.converteParaPersonagem();
        Personagem playe2 = playerResponse2.converteParaPersonagem();
#pragma warning restore CS8604

        Arena arena = new Arena(playe1, playe2);
        List<string> retorno = arena.realizaCombate();

        PersonagensRepository.update(playe1);
        PersonagensRepository.update(playe2);

        return Results.Ok(retorno);
    }

    private string? PersonagemValido(PersonagemResponse? personagem, int numPersonagem)
    {
        if (personagem == null)
            return $"Personagem {numPersonagem} não encontrado.";
        if (!personagem.Estatisticas.isAlive())
            return $"Personagem {numPersonagem} está morto. Combate invalido.";

        return null;
    }

}
