using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafezesMarket.Models
{
    public class CarrinhoItem
    {
        public long Id { get; set; }
        public long ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public long ClienteId { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Produto Produto { get; set; }

        public CarrinhoItem()
        {

        }

        public CarrinhoItem(NovoCarrinhoItem novoCarrinhoItem)
        {
            if (novoCarrinhoItem == null)
            {
                throw new ArgumentNullException(nameof(novoCarrinhoItem));
            }

            this.ClienteId = novoCarrinhoItem.ClienteId;
            this.ProdutoId = novoCarrinhoItem.ProdutoId;
            this.Quantidade = novoCarrinhoItem.Quantidade;
        }
    }
}
