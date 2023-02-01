public static class MinhasExtensions
{
    public static void AddMethods(this WebApplication app)
    {
        app.MapMethods(MeuGet.Template, MeuGet.Methods, MeuGet.Handle);
        app.MapMethods(MeuGetSimples.Template, MeuGetSimples.Methods, MeuGetSimples.Handle);

        app.MapMethods(MeuPut.Template, MeuPut.Methods, MeuPut.Handle);

        app.MapMethods(MeuPost.Template, MeuPost.Methods, MeuPost.Handle);

        app.MapMethods(MeuDelete.Template, MeuDelete.Methods, MeuDelete.Handle);
    }
}