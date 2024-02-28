namespace Economix.Tests;
public class LeituraArquivoIntegrationTests
{

    [Fact]
    public void HelloWorld_GET_Result_Is_Ok_With_HelloWorld()
    {
        var file = "2024028.TXT";
        Assert.Equal("2024028.TXT", file);

        var path = $"./Files/{file}";
        var fullPath = Path.GetFullPath(path);

        var lines = File.ReadAllLines(fullPath);
        int index = 0;
        foreach (var line in lines)
        {
            if (index == lines.Length - 1)
            {
                break;
            }
            if (index > 0)
            {
                Console.WriteLine(line[0..7]);
                Console.WriteLine(line[7..9]);
                Console.WriteLine(line[9..14]);
                //14 16
                //16 21
                //21 31



            }
            index++;
        }
    }
}
public class ArquivoLeitura
{
    private readonly string _line;
    public ArquivoLeitura(string line)
    {
        Console.WriteLine(line);
        _line = line;
    }
}