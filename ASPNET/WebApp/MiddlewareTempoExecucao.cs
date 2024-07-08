using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.Extensions.Options;

public class MiddlewareTempoExecucao
{
    private readonly RequestDelegate _next;

    private readonly CronometroOptions _options;

    public MiddlewareTempoExecucao(RequestDelegate next, IOptions<CronometroOptions> options)
    {
        _next = next;
        _options = options.Value; 
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.ContentType = "text/plain charset=UTF-8";
        var sw = Stopwatch.StartNew();;
        await _next(context);
        sw.Stop();
        double tempo =0;
        switch(_options.UnidadesMedida){
            case UnidadesTempo.Nanossegundo:
                double nanosPorTick = (1000.0 * 1000.0 * 1000.0) /Stopwatch.Frequency;
                tempo = sw.ElapsedTicks * nanosPorTick;
                break;
            case UnidadesTempo.Microssegundo:
                double mircrosPorTick = (1000.0 * 1000.0) /Stopwatch.Frequency;
                tempo = sw.ElapsedTicks * mircrosPorTick;
                break;
            case UnidadesTempo.Millissegundo:
                double millisPorTick = (1000.0) /Stopwatch.Frequency;
                tempo = sw.ElapsedTicks * millisPorTick;
                break;
        }
        //var tempo = sw.ElapsedMilliseconds;
        await context.Response.WriteAsync($"\nTempo de execucao (em milli segundos): {sw.ElapsedMilliseconds}");
        await context.Response.WriteAsync($"\nTempo de execucao (em {_options.UnidadesMedida.ToString().ToLower() + 's'}):{tempo:F2}");
        
    }
}