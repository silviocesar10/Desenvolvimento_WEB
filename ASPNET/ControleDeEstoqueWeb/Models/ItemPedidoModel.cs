using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeEstoqueWeb.Models
{
    [Table("ItemPedido")]
    public class ItemPedidoModel 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPedido {get;set;}

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProduto {get;set;}

        public int Quantidade {get;set;}

        public double ValorUnitario {get;set;}

        [ForeignKey("IdPedido")]
        public PedidoModel Pedido {get;set;}

        [ForeignKey("IdPoduto")]
        public ProdutoModel Produto {get;set;}

        public double ValorItem {get =>  this.ValorUnitario * this.Quantidade;}
    }
}