public class MeuGet
{
    public static string Template => "/meuget"; //https://localhost:7157/meuget?page=3&rows=5
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(int? page, int? rows)
    {
        if (page == null || rows == null)
            return Results.BadRequest("Page ou Rows não informada.");

        List<int> lista = new List<int>();
        for (int i = 0; i < page; i++)
        {
            for (int j = 0; j < page; j++)
            {
                lista.Add(i + j);
            }
        }

        return Results.Ok(lista);
    }
}