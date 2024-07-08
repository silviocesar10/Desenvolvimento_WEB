using System.Text;

namespace PipelineConsole.Simples
{
    public class EtapaBanco : IEtapa<StringBuilder>
    {
        public StringBuilder Processar(StringBuilder entrada)
        {
            entrada.Insert(0, "[Banco]", 2);
            entrada.Insert(entrada.Length, "[Banco]", 2);
            return entrada;
        }
    }
}