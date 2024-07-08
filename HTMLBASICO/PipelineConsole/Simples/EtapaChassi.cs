using System.Text;

namespace PipelineConsole.Simples
{
    public class EtapaChassi : IEtapa<StringBuilder>
    {
        public StringBuilder Processar(StringBuilder entrada)
        {
            entrada.Append("[Chassi]");
            return entrada;
        }
    }
}