var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Ok(new Mensagem("Hello World!_GET")));
app.MapPost("/", () =>
{
    var file = "2024028.TXT";
    var path = $"./Files/{file}";
    var fullPath = Path.GetFullPath(path);

    var lines = File.ReadAllLines(fullPath);
    List<ArquivoLeitura> list = new List<ArquivoLeitura>();
    int index = 0;
    foreach (var line in lines)
    {
        if (index == lines.Length - 1)
        {
            break;
        }
        if (index > 0)
        {
            list.Add(ArquivoLeituraBuilder.Create(line, file));
        }
        index++;
    }
    //Results.Created("/", new Mensagem("Hello World!_POST"))
    return Results.Created("/", list);
});
app.MapPut("/", () => Results.Accepted("/", new Mensagem("Hello World!_PUT")));
app.MapDelete("/", () => Results.NoContent());

app.Run();
