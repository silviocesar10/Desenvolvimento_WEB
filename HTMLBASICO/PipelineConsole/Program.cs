using System;
using System.Text;
using PipelineConsole.Simples;
using PipelineConsole.Duplo;

namespace PipelineConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            smp();
            dpl();
        }

        public static void smp(){
            Console.WriteLine();
            Console.WriteLine("Pipeline simples para montagem de veiculos!!");

            var montagemVeiculo = new Pipeline<StringBuilder>();
            montagemVeiculo.AdicionarEtapa(new EtapaChassi());
            montagemVeiculo.AdicionarEtapa(new EtapaMotor());
            montagemVeiculo.AdicionarEtapa(new EtapaBanco());
            montagemVeiculo.AdicionarEtapa(new EtapaCarroceria());
            montagemVeiculo.AdicionarEtapa(new EtapaPorta());
            montagemVeiculo.AdicionarEtapa(new EtapaPintura());


            for(int i=0; i<10; i++)
            {
                var veiculo = montagemVeiculo.Processar(new StringBuilder());
                Console.WriteLine($"Veiculo {i + 1:D2}: {veiculo.ToString()}");
            }
        }
        public static void dpl(){
            Console.WriteLine();
            Console.WriteLine("Pipeline duplo para montagem de veiculos!!");

            var montagemVeiculo = new Pipeline<StringBuilder>();
            montagemVeiculo.AdicionarEtapa(new EtapaChassiDuplo());
            montagemVeiculo.AdicionarEtapa(new EtapaMotorDuplo());
            montagemVeiculo.AdicionarEtapa(new EtapaBancoDuplo());
            montagemVeiculo.AdicionarEtapa(new EtapaCarroceriaDuplo());
            montagemVeiculo.AdicionarEtapa(new EtapaPortaDuplo());
            montagemVeiculo.AdicionarEtapa(new EtapaPinturaDuplo());


            for(int i=0; i<10; i++)
            {
                var veiculo = montagemVeiculo.Processar(new StringBuilder());
                Console.WriteLine($"Veiculo {i + 1:D2}: {veiculo.ToString()}");
            }
        }
    }
}
