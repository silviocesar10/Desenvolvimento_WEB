using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

public class EndpointConsultaCep
{
    public  async Task Endpoint(HttpContext context, IFormatadorEndereco formatador)
    {
        string cep = context.Request.RouteValues["cep"] as string ?? "01001000";

        var objetoCep = await ConsultaCep(cep);
        if (objetoCep == null)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
        else
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            await formatador.Formatar(context, objetoCep);
        }
    }

    public static async Task<JsonCep> ConsultaCep(string cep)
    {
        var url = $"https://viacep.com.br/ws/{cep}/json";
        var cliente = new HttpClient();
        cliente.DefaultRequestHeaders.Add("User-Agent", "Middleware Consulta CEP");
        var response = await cliente.GetAsync(url);

        var dadosCEP = await response.Content.ReadAsStringAsync();
        dadosCEP = dadosCEP.Replace("?(", "").Replace(");", "").Trim();
        return dadosCEP.Contains("\"erro\":") ? null :
            JsonConvert.DeserializeObject<JsonCep>(dadosCEP);
    }
}
