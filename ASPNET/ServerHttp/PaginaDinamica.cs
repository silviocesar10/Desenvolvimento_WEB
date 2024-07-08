using System.Collections.Generic;
using System;
using System.Text;
abstract class PaginaDinamica
{
    public string htmlModelo {get; set;}
    
    public virtual byte[] Get(SortedList<string, string> parametros)
    {
        return Encoding.UTF8.GetBytes(this.htmlModelo); 
    }
    public virtual byte[] Post(SortedList<string, string> parametros)
    {
        return Encoding.UTF8.GetBytes(this.htmlModelo); 
    }

}