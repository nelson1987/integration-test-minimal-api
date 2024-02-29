# integration-test-minimal-api
Utilizarei nesse projeto 4 das integrações que mais acontecem no SFN: Banco de dados, spis externas, mensageria e arquivo posicional .txt
## História
No FishBank a cada 4 horas um robô envia uma requisição GET para o endpoint "/", para verificar se nesse momento está sendo executado o processo de compensação de transferências.
Se ele receber uma resposta de que no momento não há processo sendo executado. O robô enviará uma requisição POST para o endpoint "/", contendo o dia da próxima compensação, e o usuário responsável pela execução.

Compensação: A Compensação é um processo que consiste em ler todos os arquivos nomeados com o prefixo, que foi enviado. Em seguida persistir os dados das transferências compensadas. Para que na sequência seja enviada uma requisição para uma API Externa que é a comunicação com a autorizadora de valores. Para por fim publicar em uma fila/tópico para enviar para outra área do FishBank. Que após realizar todas as validações publicará numa outra fila/tópico que será consumida pela nossa área, e atualizaremos os dados da transação no banco já persistido.

Lembrando utilizaremos:
* Um servidor de mensageria(Kafka ou RabbitMq)
* Um banco NoSql, para ser o banco de leitura
* Um banco Relacional, para ser o banco de escrita
* E um servidor HTTP para testar as rotas da API Externa.

Nossa intenção é demonstrar como realizar um teste de integração com todos esses processos

## Arquivo Posicional
### Header
| Numero | Posicao | Picture | Conteudo do Campo | Especificações |
| ------ | ------ | ------ | ------ | ------ |
| 1 | 001-007 | 9(007) | Controle de Header | Preencher com Zeros("0000000") |
| 2 | [plugins/github/README.md][PlGh] |
### Body
| Numero | Posicao | Picture | Conteudo do Campo | Especificações |
| ------ | ------ | ------ | ------ | ------ |
| 1 | 001-007 | 9(007) | Controle de Body | --- |
| 2 | [plugins/github/README.md][PlGh] |
| 3 | [plugins/googledrive/README.md][PlGd] |
| 4 | [plugins/onedrive/README.md][PlOd] |
| 5 | [plugins/medium/README.md][PlMe] |
| 6 | [plugins/googleanalytics/README.md][PlGa] |
### Footer
| Numero | Posicao | Picture | Conteudo do Campo | Especificações |
| ------ | ------ | ------ | ------ | ------ |
| 1 | 001-007 | 9(007) | Controle de Footer | Preencher com Noves("9999999") |
| 2 | [plugins/github/README.md][PlGh] |

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

### bibliografia
https://macoratti.net/21/10/aspn6_estrminapi1.htm