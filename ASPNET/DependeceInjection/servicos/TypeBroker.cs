public static class TypeBrokwer
{
    private static IFormatadorEndereco instanciaCompartilhada = new EnderecoHtml();
    public static IFormatadorEndereco FormatadorEndereco => instanciaCompartilhada; 
}