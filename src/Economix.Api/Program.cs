var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Ok(new Mensagem("Hello World!_GET")));
app.MapPost("/", () =>
{
    Leitura leitura = new Leitura();
    var list = leitura.Ler("2024028.TXT");
    //Results.Created("/", new Mensagem("Hello World!_POST"))
    return Results.Created("/", list);
});
app.MapPut("/", () => Results.Accepted("/", new Mensagem("Hello World!_PUT")));
app.MapDelete("/", () => Results.NoContent());

app.Run();
