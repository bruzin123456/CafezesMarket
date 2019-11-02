using System.Collections.Generic;

namespace CafezesMarket.Models
{
    public class Produto
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Categoria { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public bool Ativo { get; set; }

        public virtual List<ProdutoFoto> Fotos { get; set; }
        public virtual List<PedidoItem> PedidosItems { get; set; }
    }
}