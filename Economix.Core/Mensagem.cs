namespace Economix.Core;


public record Mensagem(string? Message);
public static class ArquivoLeituraBuilder
{
    public static ArquivoLeitura Create(string line, string file)
    {
        return new ArquivoLeitura()
        {
            FileName = file,
            TipoCreditante = int.Parse(line[7..9]),
            CreditanteId = int.Parse(line[9..14]),
            TipoDebitante = int.Parse(line[14..16]),
            DebitanteId = int.Parse(line[16..21]),
            Valor = decimal.Parse($"{line[21..31]},{line[31..33]}")
        };
    }
}
public class Leitura
{
    public List<ArquivoLeitura>? Ler(string file)
    {
        var path = $"./Files/{file}";
        var fullPath = Path.GetFullPath(path);

        var lines = File.ReadAllLines(fullPath);
        List<ArquivoLeitura> arquivo = new List<ArquivoLeitura>();
        int index = 0;
        foreach (var line in lines)
        {
            if (index == lines.Length - 1)
            {
                break;
            }
            if (index > 0)
            {
                arquivo.Add(ArquivoLeituraBuilder.Create(line, file));
            }
            index++;
        }

        return arquivo;
    }
}
public class ArquivoLeitura
{
    public required string FileName { get; set; }
    public int TipoCreditante { get; set; }
    public int CreditanteId { get; set; }
    public int TipoDebitante { get; set; }
    public int DebitanteId { get; set; }
    public decimal Valor { get; set; }
}