using System;
using System.Collections.Generic;

namespace CafezesMarket.Models
{
    public class Pedido
    {
        public long Id { get; set; }
        public long ClienteId { get; set; }
        public int SituacaoId { get; set; }
        public DateTime Emissao { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual PedidoSituacao Situacao { get; set; }
        public virtual List<PedidoItem> Itens { get; set; }
    }
}