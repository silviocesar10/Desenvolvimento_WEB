using System.Collections.Generic;

namespace PipelineConsole.Simples
{
    public interface IEtapa<T>
    {
        T Processar(T entrada);
    }
}