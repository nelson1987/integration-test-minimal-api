var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!_GET");
app.MapPost("/", () => "Hello World!_POST");
app.MapPut("/", () => "Hello World!_PUT");
app.MapDelete("/", () => "Hello World!_DELETE");

app.Run();
