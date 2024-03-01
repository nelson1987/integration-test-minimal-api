using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Economix.Tests;
internal class ApiFixture : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder.UseEnvironment("Testing");
    //.ConfigureServices(services => services.AddMassTransitTestHarness());
}
public class HelloWorldIntegrationTests : IDisposable
{
    private readonly ApiFixture Server;
    public readonly HttpClient _httpClient;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITransferenciaRepository _transferenciaRepository;
    private readonly ITesourariaEventProducer _eventClient;
    private readonly IAutorizadorService _autorizador;

    public HelloWorldIntegrationTests()
    {
        Server = new ApiFixture();
        //var webAppFactory = new ApiFixture();
        _httpClient = Server.CreateClient();
        _usuarioRepository = Server.Services.GetRequiredService<IUsuarioRepository>();
        _autorizador = Server.Services.GetRequiredService<IAutorizadorService>();
    }

    [Fact]
    public async Task HelloWorld_GET_Result_Is_Ok_With_HelloWorld()
    {
        var response = await _httpClient.GetAsync("/");
        response.EnsureSuccessStatusCode();
        Assert.Equal((int)HttpStatusCode.OK, (int)response.StatusCode);

        var resultJson = await response.Content.ReadAsStringAsync();
        Mensagem result = System.Text.Json.JsonSerializer.Deserialize<Mensagem>(resultJson,
            new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web))!;
        Assert.Equal("Hello World!_GET", result.Message);
    }

    [Fact]
    public async Task HelloWorld_POST_Result_Is_BadRequest_Cause_Debitante_IsNull_With_HelloWorld()
    {
        _usuarioRepository.Insert(new Usuario() { Id = 1, Tipo = 1 });
        _usuarioRepository.Insert(new Usuario() { Id = 2, Tipo = 2 });
        _usuarioRepository.Insert(new Usuario() { Id = 3, Tipo = 1 });
        Assert.True(_autorizador.TransferenciaAutorizada(1, 0.01M));
        Assert.True(_autorizador.TransferenciaAutorizada(1, 95.23M));
        var request = new StringContent("", Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsJsonAsync("/", request);
        response.EnsureSuccessStatusCode();
        Assert.Equal((int)HttpStatusCode.Created, (int)response.StatusCode);

        var resultJson = await response.Content.ReadAsStringAsync();
        Mensagem result = System.Text.Json.JsonSerializer.Deserialize<Mensagem>(resultJson,
            new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web))!;
        Assert.Equal("Hello World!_POST", result.Message);
        //_transferenciaRepository.GetById();
    }

    [Fact]
    public async Task HelloWorld_POST_Result_Is_BadRequest_Cause_Autorizador_IsFalse_With_HelloWorld()
    {
        _usuarioRepository.Insert(new Usuario() { Id = 1, Tipo = 1 });
        _usuarioRepository.Insert(new Usuario() { Id = 2, Tipo = 2 });
        _usuarioRepository.Insert(new Usuario() { Id = 3, Tipo = 1 });
        Assert.True(_autorizador.TransferenciaAutorizada(1, 0.01M));
        Assert.False(_autorizador.TransferenciaAutorizada(1, 95.23M));
        var request = new StringContent("", Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsJsonAsync("/", request);
        Assert.Equal((int)HttpStatusCode.BadRequest, (int)response.StatusCode);
    }

    [Fact]
    public async Task HelloWorld_POST_Result_Is_Created_With_HelloWorld()
    {
        var request = new StringContent("", Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsJsonAsync("/", request);
        //response.EnsureSuccessStatusCode();
        Assert.Equal((int)HttpStatusCode.BadRequest, (int)response.StatusCode);

        //var resultJson = await response.Content.ReadAsStringAsync();
        ////List<ArquivoLeitura> result = System.Text.Json.JsonSerializer.Deserialize<List<ArquivoLeitura>>(resultJson,
        ////  new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web))!;
        ////Assert.Equal(2, result.Count);
        //Mensagem result = System.Text.Json.JsonSerializer.Deserialize<Mensagem>(resultJson,
        //    new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web))!;
        //Assert.Equal("Hello World!_POST", result.Message);
    }

    [Fact]
    public async Task HelloWorld_PUT_Result_Is_Accepted_With_HelloWorld()
    {
        var request = new StringContent("", Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsJsonAsync("/", request);
        response.EnsureSuccessStatusCode();
        Assert.Equal((int)HttpStatusCode.Accepted, (int)response.StatusCode);

        var resultJson = await response.Content.ReadAsStringAsync();
        Mensagem result = System.Text.Json.JsonSerializer.Deserialize<Mensagem>(resultJson,
            new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web))!;
        Assert.Equal("Hello World!_PUT", result.Message);
    }

    [Fact]
    public async Task HelloWorld_DELETE_Result_Is_NoContent()
    {
        var response = await _httpClient.DeleteAsync("/");
        response.EnsureSuccessStatusCode();

        Assert.Equal((int)HttpStatusCode.NoContent, (int)response.StatusCode);
    }

    public void Dispose()
    {
        _usuarioRepository.DeleteAll();
    }
}