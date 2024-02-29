using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCore();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new Mensagem("Hello World!_GET")));
app.MapPost("/", ([FromServices] ILeituraArquivosHandler handler) =>
{
    handler.Handle();
    return Results.Created("/", new Mensagem("Hello World!_POST"));
    //return Results.Created("/", list);
});
app.MapPut("/", () => Results.Accepted("/", new Mensagem("Hello World!_PUT")));
app.MapDelete("/", () => Results.NoContent());

app.Run();
