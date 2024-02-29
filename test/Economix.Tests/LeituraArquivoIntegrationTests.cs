namespace Economix.Tests;
public class LeituraArquivoIntegrationTests
{

    [Fact]
    public void HelloWorld_GET_Result_Is_Ok_With_HelloWorld()
    {
        Leitura leitura = new Leitura();
        var file = "2024028.TXT";
        Assert.Equal("2024028.TXT", file);

        var arquivos = leitura.Ler(file);
        Assert.True(arquivos!.Any());
    }
}
public class ArquivoLeituraUnitTests
{
    [Theory]
    [InlineData("000000101000010200011000000000001", "20190123.txt")]
    public void HelloWorld_GET_Result_Is_Ok_With_HelloWorld(string? line, string? file)
    {
        ArquivoLeitura arquivo = ArquivoLeituraBuilder.Create(line!, file!);
        Assert.Equal("20190123.txt", arquivo.FileName);
        Assert.Equal(1, arquivo.TipoCreditante);
        Assert.Equal(1, arquivo.CreditanteId);
        Assert.Equal(2, arquivo.TipoDebitante);
        Assert.Equal(11, arquivo.DebitanteId);
        Assert.Equal(0.01M, arquivo.Valor);
    }
}