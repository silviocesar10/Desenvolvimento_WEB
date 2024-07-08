using System.Collections.Generic;
using PipelineConsole.Simples;

namespace PipelineConsole.Duplo
{
    public interface IEtapaDuplo<T> :IEtapa<T>
    {
       IEtapaDuplo<T> ProximaEtapa {get; set;}
    }
}