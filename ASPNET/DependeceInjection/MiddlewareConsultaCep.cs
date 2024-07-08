using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public class MiddlewareConsultaCep
{
    private readonly RequestDelegate next;


    public MiddlewareConsultaCep() { }

    public MiddlewareConsultaCep(RequestDelegate nextMiddleware)
    {
        next = nextMiddleware;
    }

    public async Task Invoke(HttpContext context,
        IFormatadorEndereco formatador1,
        IFormatadorEndereco formatador2
    )
    {
        //checa se a URL é compatível com o middleware
        if (context.Request.Path.StartsWithSegments("/mw/classe"))
        {
            //captura o valor do CEP da URL
            string[] segmentos = context.Request.Path.ToString().Split('/',
                StringSplitOptions.RemoveEmptyEntries);
            string cep = segmentos.Length > 2 ? segmentos[2] : "01001000";

            var objetoCep1 = await ConsultaCep(cep);
            var objetoCep2 = await ConsultaCep("22290270");
            context.Response.ContentType = "text/html; charset=utf-8";
            await formatador1.Formatar(context, objetoCep1);
            await formatador2.Formatar(context, objetoCep2);
        }

        if (next != null)
        {
            await next(context);
        }
    }

    private async Task<JsonCep> ConsultaCep(string cep)
    {
        var url = $"https://viacep.com.br/ws/{cep}/json";
        var cliente = new HttpClient();
        cliente.DefaultRequestHeaders.Add("User-Agent", "Middleware Consulta CEP");
        var response = await cliente.GetAsync(url);

        var dadosCEP = await response.Content.ReadAsStringAsync();
        dadosCEP = dadosCEP.Replace("?(", "").Replace(");", "").Trim();
        return JsonConvert.DeserializeObject<JsonCep>(dadosCEP);
    }
}
