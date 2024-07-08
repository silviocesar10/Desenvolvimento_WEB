using System.Text;

namespace PipelineConsole.Simples
{
    public class EtapaPorta : IEtapa<StringBuilder>
    {
        public StringBuilder Processar(StringBuilder entrada)
        {
            entrada.Insert(0, "[Porta]", 2);
            entrada.Insert(entrada.Length, "[Porta]", 2);
            return entrada;
        }
    }
}