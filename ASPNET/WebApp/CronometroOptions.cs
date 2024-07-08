using System;

public class CronometroOptions
{
    public int qtdUnidades = Enum.GetValues(typeof(UnidadesTempo)).Length;

    public UnidadesTempo UnidadesMedida {get; set;}
}