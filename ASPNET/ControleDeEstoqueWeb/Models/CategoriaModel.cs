using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ControleDeEstoqueWeb.Models
{
    public class CategoriaModel
    {
        [Key]
        public int IdCategoria {get; set;}

        [Required, MaxLength(128)]
        public string Nome {get;set;}

        public ICollection<ProdutoModel> Produto {get;set;}
    }
}