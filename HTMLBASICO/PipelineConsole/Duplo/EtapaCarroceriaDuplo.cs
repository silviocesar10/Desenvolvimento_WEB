using System.Text;

namespace PipelineConsole.Duplo
{
    public class EtapaCarroceriaDuplo : IEtapaDuplo<StringBuilder>
    {
        public IEtapaDuplo<StringBuilder> ProximaEtapa {get; set;}
        public StringBuilder Processar(StringBuilder entrada)
        {
            entrada.Insert(0, "[Carroceria]", 1);
            entrada.Insert(entrada.Length, "[Carroceria]", 1);
            return entrada;
        }
    }
}