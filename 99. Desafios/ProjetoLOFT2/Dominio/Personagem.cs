using System.Text.RegularExpressions;
using Dominio.Classes;
using Flunt.Notifications;
using Flunt.Validations;
using Microsoft.AspNetCore.Components.Forms;

namespace Dominio;

public class Personagem : Notifiable<Notification>
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public BaseClass Estatisticas { get; private set; }

    public Personagem(int id, string nome, BaseClass classe)
    {
        Id = id;
        Nome = nome;
        Estatisticas = classe;
        Validar();
    }
    private void Validar()
    {
        var contract = new Contract<Personagem>()
            .IsNotNull(Id, "Id", "Erro ao informar a Id.")
            .IsNotNullOrEmpty(Nome, "Nome", "Nome tem que estar preenchido.")
            .IsLowerOrEqualsThan(Nome, 15, "Nome", "Nome dever no maximo 15 caracteres.");


        bool somenteCaracteresValidos = Regex.IsMatch(Nome, @"^[a-zA-Z_]+$");
        if (!somenteCaracteresValidos)
        {
            contract.AddNotification("Nome", "Nome só pode conter letras e _(underline)");
        }

        AddNotifications(contract);
    }




}