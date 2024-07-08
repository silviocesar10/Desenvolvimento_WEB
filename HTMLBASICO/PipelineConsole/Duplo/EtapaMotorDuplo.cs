using System.Text;

namespace PipelineConsole.Duplo
{
    public class EtapaMotorDuplo : IEtapaDuplo<StringBuilder>
    {
        public IEtapaDuplo<StringBuilder> ProximaEtapa {get; set;}
        public StringBuilder Processar(StringBuilder entrada)
        {
            entrada.Append("[Motor]");
            entrada = ProximaEtapa?.Processar(entrada) ?? entrada;
            return entrada;
        }
    }
}