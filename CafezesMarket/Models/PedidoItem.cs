namespace CafezesMarket.Models
{
    public class PedidoItem
    {
        public long Id { get; set; }
        public long PedidoId { get; set; }
        public long ProdutoId { get; set; }
        public decimal Preco { get; set; }
        public decimal Desconto { get; set; }
        public int Quantidade { get; set; }

        public virtual Pedido Pedido { get; set; }
        public virtual Produto Produto { get; set; }
    }
}