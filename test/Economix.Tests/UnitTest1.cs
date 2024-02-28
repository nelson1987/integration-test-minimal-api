namespace Economix.Tests;

public class MinimalIntegrationTests
{
    [Fact]
    public void HelloWorld_Result_Is_HelloWorld()
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        var httpClient = webAppFactory.CreateDefaultClient();

        var response = await httpClient.GetAsync("/");
        var result = await response.Content.ReadAsStringAsync();

        Assert.Equal("Hello World!", result);
    }
}