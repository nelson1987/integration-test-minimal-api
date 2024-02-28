var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Ok("Hello World!_GET"));
app.MapPost("/", () => Results.Created("/", "Hello World!_POST"));
app.MapPut("/", () => Results.Accepted("/", "Hello World!_PUT"));
app.MapDelete("/", () => Results.NoContent());

app.Run();
