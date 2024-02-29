using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new Mensagem("Hello World!_GET")));
app.MapPost("/", ([FromServices] IUsuarioRepository usuarioRepository) =>
{
    Leitura leitura = new Leitura();
    var list = leitura.Ler("2024028.TXT");
    foreach (var arquivo in list)
    {
        var creditante = usuarioRepository.GetFilter(arquivo.TipoCreditante, arquivo.CreditanteId);
        if (creditante == null) return Results.NotFound();
        var debitante = usuarioRepository.GetFilter(arquivo.TipoCreditante, arquivo.CreditanteId);
        if (debitante == null) return Results.NotFound();
    }
    //Results.Created("/", new Mensagem("Hello World!_POST"))
    return Results.Created("/", list);
});
app.MapPut("/", () => Results.Accepted("/", new Mensagem("Hello World!_PUT")));
app.MapDelete("/", () => Results.NoContent());

app.Run();
