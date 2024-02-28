# integration-test-minimal-api
Utilizarei nesse projeto 4 das integrações que mais acontecem no SFN: Banco de dados, spis externas, mensageria e arquivo posicional .txt

## Criar solution
```sh
dotnet new sln -n Economix
dotnet new gitignore
```

## Criar projeto de WebApi
```sh
dotnet new gitignore
mkdir src
cd src
dotnet new web -o Economix.Api
cd Economix.Api
dotnet run
```

## Parar Api
```sh
ctrl+C
```

## Submeter para GitHub
```sh
git pull
git add .
git commit -m "Mensagem Commit"
git push
```

## Criar Projeto Test
```sh
mkdir test
cd test
dotnet new xunit -n Economix.Tests
cd Economix.Tests
dotnet tests
```

## Incluir frameworks em Tests
```sh
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Microsoft.AspNetCore.TestHost
dotnet tests
```

## Incluir Projetos em Solution

```sh
cd ../
dotnet sln Economix.sln add src/Economix.Api/Economix.Api.csproj -s src
dotnet sln Economix.sln add test/Economix.Tests/Economix.Tests.csproj -s tests
```

## Incluir Primeiro Teste
```code
public class HelloWorldIntegrationTests
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
```
## Dar visibilidade a class Program de Api em Test
Adicionar em Api.csproj
```code
	<ItemGroup>
		<InternalsVisibleTo Include="Economix.Tests"/>
	</ItemGroup>
```
* Executar o Run de Tests

```sh
dotnet dev-certs https --trust
```