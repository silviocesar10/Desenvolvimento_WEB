using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;

public class EnderecoTextual : IFormatadorEndereco
{
    private int contadorDeUso =0;
    private static EnderecoTextual instanciaCompartilhada;
    public async Task Formatar(HttpContext context, IEndereco endereco)
    {
        StringBuilder conteudo = new StringBuilder();
        conteudo.Append($"CEP {endereco.CEP}\n");
        conteudo.Append($"Logradouro {endereco.Logradouro}\n");
        conteudo.Append($"Complemento {endereco.Complemento}\n");
        conteudo.Append($"Bairro {endereco.Bairro}\n");
        conteudo.Append($"Cidade/UF {endereco.Localidade}/{endereco.Estado}\n");
        conteudo.Append($"\nFormatador usado {++contadorDeUso} vez(es).\n");
        context.Response.ContentType = "text/plain; charset=utf-8";
        await context.Response.WriteAsync(conteudo.ToString());
    }
    public static EnderecoTextual Singleton
    {
        get
        {
            if(instanciaCompartilhada == null)
            {
                instanciaCompartilhada = new EnderecoTextual();
            }
            return instanciaCompartilhada;
        }
    }
}