using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCore();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new Mensagem("Hello World!_GET")));
app.MapPost("/", ([FromServices] ILeituraArquivosHandler handler) =>
{

    var result = handler.Handle();
    return result.IsSuccess
        ? Results.Created("/", new Mensagem("Hello World!_POST"))
        : Results.BadRequest();
    //return Results.Created("/", list);
});
app.MapPut("/", () => Results.Accepted("/", new Mensagem("Hello World!_PUT")));
app.MapDelete("/", () => Results.NoContent());

app.Run();
