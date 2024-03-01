using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Economix.Core;
public static class Dependencies
{
    public static void AddCore(this IServiceCollection service) => service
        .AddScoped<ILeituraArquivosHandler, LeituraArquivosHandler>()
        .AddScoped<IUsuarioRepository, UsuarioRepository>()
        .AddScoped<ITransferenciaRepository, TransferenciaRepository>()
        .AddScoped<ITesourariaEventProducer, TesourariaEventProducer>()
        .AddScoped<IAutorizadorService, AutorizadorService>();

}

public interface ILeituraArquivosHandler
{
    Result Handle();
}
public class LeituraArquivosHandler : ILeituraArquivosHandler
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITransferenciaRepository _transferenciaRepository;
    private readonly ITesourariaEventProducer _eventClient;
    private readonly IAutorizadorService _autorizador;

    public LeituraArquivosHandler(IUsuarioRepository usuarioRepository,
        ITransferenciaRepository transferenciaRepository,
        ITesourariaEventProducer eventClient,
        IAutorizadorService autorizador)
    {
        _usuarioRepository = usuarioRepository;
        _transferenciaRepository = transferenciaRepository;
        _eventClient = eventClient;
        _autorizador = autorizador;
    }
    public Result Handle()
    {
        Leitura leitura = new Leitura();
        var list = leitura.Ler("2024028.TXT");
        foreach (var arquivo in list!)
        {
            //Valor
            if (arquivo.Valor == 0) return Result.Fail("Valor não pode ser zero");
            //Debitante
            var debitante = _usuarioRepository.GetFilter(arquivo.TipoCreditante, arquivo.CreditanteId);
            if (debitante == null) return Result.Fail("Debitante não encontrado");
            //Creditante
            var creditante = _usuarioRepository.GetFilter(arquivo.TipoCreditante, arquivo.CreditanteId);
            if (creditante == null) return Result.Fail("Creditante não encontrado");

            //Autorizador Externo
            bool autorizada = _autorizador.TransferenciaAutorizada(arquivo.CreditanteId, arquivo.Valor);
            if (!autorizada) return Result.Fail("Transferência não autorizada");
            //Inserção de Transferência in MongoDb
            var transferencia = new Transferencia()
            {
                creditante = creditante,
                debitante = debitante,
                valor = arquivo.Valor
            };
            _transferenciaRepository.Insert(transferencia);
            //Envio para Mensageria
            _eventClient.SendTransferencia(new InclusaoTranferenciaEvent(transferencia));
        }
        return Result.Ok();
    }
}


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
                break;

            if (index > 0)
                arquivo.Add(ArquivoLeituraBuilder.Create(line, file));

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

public class Usuario
{
    public int Id { get; set; }
    public int Tipo { get; set; }
}

public interface IAutorizadorService
{
    bool TransferenciaAutorizada(int idDebitante, decimal valor);
}
public class AutorizadorService : IAutorizadorService
{
    public bool TransferenciaAutorizada(int idDebitante, decimal valor)
    {
        return true;
    }
}

public interface ITesourariaEventProducer
{
    void SendTransferencia(InclusaoTranferenciaEvent @event);
}
public class TesourariaEventProducer : ITesourariaEventProducer
{
    public void SendTransferencia(InclusaoTranferenciaEvent @event)
    {
    }
}

public interface ITransferenciaRepository
{
    void Insert(Transferencia transferencia);
}
public class TransferenciaRepository : ITransferenciaRepository
{
    public void Insert(Transferencia transferencia)
    {
    }
}

public interface IUsuarioRepository
{
    Usuario? GetFilter(int tipoUsuario, int usuarioId);
}
public class UsuarioRepository : IUsuarioRepository
{
    private readonly IMongoCollection<Usuario> _ticketsCollection;
    //TODO: Implementar UnitOfWork
    public UsuarioRepository()
    {
        var mongoClient = new MongoClient("mongodb://root:example@localhost:27017/");
        var database = mongoClient.GetDatabase("Economix");
        _ticketsCollection = database.GetCollection<Usuario>(nameof(Usuario));
    }
    public Usuario? GetFilter(int tipoUsuario, int usuarioId)
    {

        var collection = _ticketsCollection.Find(x => x.Id == usuarioId && x.Tipo == tipoUsuario);
        return collection.FirstOrDefault();

        //return new Usuario() { Id = usuarioId, Tipo = tipoUsuario };
    }
}

public class Transferencia
{
    public required Usuario debitante { get; set; }
    public required Usuario creditante { get; set; }
    public decimal valor { get; set; }
}
public record InclusaoTranferenciaEvent(Transferencia Transferencia);