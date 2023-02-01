using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;

public static class MinhasExtensions
{
    public static void AddMethods(this WebApplication app)
    {
        //GETS
        app.MapMethods(BuscaPersonagem.Template, BuscaPersonagem.Methods, BuscaPersonagem.Handle);
        app.MapMethods(ListaPersonagens.Template, ListaPersonagens.Methods, ListaPersonagens.Handle);

        //POSTS
        app.MapMethods(NovoPersonagem.Template, NovoPersonagem.Methods, NovoPersonagem.Handle);
    }

    public static Dictionary<string, string[]> ConverteParaProblemDetails(this IReadOnlyCollection<Notification> notifications)
    {
        return notifications.GroupBy(g => g.Key)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(x => x.Message).ToArray()
                            );
    }
}