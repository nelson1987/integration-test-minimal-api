using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;

namespace Economix.Tests;

public class HelloWorldIntegrationTests
{
    public readonly HttpClient httpClient;
    public HelloWorldIntegrationTests()
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        httpClient = webAppFactory.CreateDefaultClient();
    }

    [Fact]
    public async Task HelloWorld_GET_Result_Is_Ok_With_HelloWorld()
    {
        var response = await httpClient.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        Assert.Equal((int)HttpStatusCode.OK, (int)response.StatusCode);
        Assert.Equal("\"Hello World!_GET\"", result);
    }

    [Fact]
    public async Task HelloWorld_POST_Result_Is_Created_With_HelloWorld()
    {
        var request = new StringContent("", Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        Assert.Equal((int)HttpStatusCode.Created, (int)response.StatusCode);
        Assert.Equal("\"Hello World!_POST\"", result);
    }

    [Fact]
    public async Task HelloWorld_PUT_Result_Is_Accepted_With_HelloWorld()
    {
        var request = new StringContent("", Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync("/", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        Assert.Equal((int)HttpStatusCode.Accepted, (int)response.StatusCode);
        Assert.Equal("\"Hello World!_PUT\"", result);
    }

    [Fact]
    public async Task HelloWorld_DELETE_Result_Is_NoContent()
    {
        var response = await httpClient.DeleteAsync("/");
        response.EnsureSuccessStatusCode();

        Assert.Equal((int)HttpStatusCode.NoContent, (int)response.StatusCode);
    }
}