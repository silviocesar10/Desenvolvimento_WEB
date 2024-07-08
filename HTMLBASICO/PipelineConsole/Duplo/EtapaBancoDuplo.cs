using System.Text;

namespace PipelineConsole.Duplo
{
    public class EtapaBancoDuplo : IEtapaDuplo<StringBuilder>
    {
        public IEtapaDuplo<StringBuilder> ProximaEtapa {get; set;}
        public StringBuilder Processar(StringBuilder entrada)
        {
            entrada.Insert(0, "[Banco]", 2);
            entrada.Insert(entrada.Length, "[Banco]", 2);
            return entrada;
        }
    }
}