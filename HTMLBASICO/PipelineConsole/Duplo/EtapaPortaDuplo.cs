using System.Text;

namespace PipelineConsole.Duplo
{
    public class EtapaPortaDuplo : IEtapaDuplo<StringBuilder>
    {
        public IEtapaDuplo<StringBuilder> ProximaEtapa {get; set;}
        public StringBuilder Processar(StringBuilder entrada)
        {
            entrada.Insert(0, "[Porta]", 2);
            entrada.Insert(entrada.Length, "[Porta]", 2);
            entrada = ProximaEtapa?.Processar(entrada) ?? entrada;
            int portasEsquerda = entrada.ToString().IndexOf("[PORTA]");
            entrada.Insert(portasEsquerda, "[MACANETA]", 2);
            int portasDireita = entrada.ToString().LastIndexOf("[PORTA]" + 7);
            entrada.Insert(portasDireita, "[MACANETA]", 2);
            return entrada;
        }
    }
}