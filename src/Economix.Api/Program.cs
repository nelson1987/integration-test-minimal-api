using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCore();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new Mensagem("Hello World!_GET")));
app.MapPost("/", ([FromServices] IUsuarioRepository usuarioRepository,
    [FromServices] ITransferenciaRepository transferenciaRepository,
    [FromServices] ITesourariaEventProducer eventClient) =>
{
    Leitura leitura = new Leitura();
    var list = leitura.Ler("2024028.TXT");
    foreach (var arquivo in list!)
    {
        //Creditante
        var creditante = usuarioRepository.GetFilter(arquivo.TipoCreditante, arquivo.CreditanteId);
        if (creditante == null) return Results.NotFound();
        //Debitante
        var debitante = usuarioRepository.GetFilter(arquivo.TipoCreditante, arquivo.CreditanteId);
        if (debitante == null) return Results.NotFound();
        //Valor
        if (arquivo.Valor == 0) return Results.BadRequest();
        var transferencia = new Transferencia()
        {
            creditante = creditante,
            debitante = debitante,
            valor = arquivo.Valor
        };
        transferenciaRepository.Insert(transferencia);
        eventClient.SendTransferencia(new InclusaoTranferenciaEvent(transferencia));
    }
    //Results.Created("/", new Mensagem("Hello World!_POST"))
    return Results.Created("/", list);
});
app.MapPut("/", () => Results.Accepted("/", new Mensagem("Hello World!_PUT")));
app.MapDelete("/", () => Results.NoContent());

app.Run();
