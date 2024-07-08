using System.Text;

namespace PipelineConsole.Simples
{
    public class EtapaMotor : IEtapa<StringBuilder>
    {
        public StringBuilder Processar(StringBuilder entrada)
        {
            entrada.Append("[Motor]");
            return entrada;
        }
    }
}