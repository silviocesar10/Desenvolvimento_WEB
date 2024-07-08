using System.Collections.Generic;

namespace EnquetesWEB.Models
{
    public class Repositorio
    {
        private static List<Resposta> respostas = new List<Resposta>();
        public static IEnumerable<Resposta> Respostas {get{ return respostas;}}

        public static void AdicionarResposta(Resposta r)
        {
            respostas.Add(r);
        }

        static Repositorio()
        {
            addTempTesteContent();
        }

        private static void addTempTesteContent()
        {
            respostas.Add(new Resposta()
            {Nome = "Fulano", Email ="fulano@email.com", Sim =true});
            respostas.Add(new Resposta()
            {Nome = "Ciclano", Email ="fulano@email.com", Sim =true});
            respostas.Add(new Resposta()
            {Nome = "Beltrano", Email ="fulano@email.com", Sim =false});
            respostas.Add(new Resposta()
            {Nome = "Joao", Email ="joao@email.com", Sim =true});
            respostas.Add(new Resposta()
            {Nome = "Jose", Email ="jose@email.com", Sim =false});
        }
    }
}