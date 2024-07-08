using System.Text;

namespace PipelineConsole.Duplo
{
    public class EtapaChassiDuplo : IEtapaDuplo<StringBuilder>
    {
        public IEtapaDuplo<StringBuilder> ProximaEtapa {get; set;}
        public StringBuilder Processar(StringBuilder entrada)
        {
            entrada.Append("[Chassi]");
            entrada = ProximaEtapa?.Processar(entrada) ?? entrada;
            return entrada;
        }
    }
}