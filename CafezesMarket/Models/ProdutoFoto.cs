using System;

namespace CafezesMarket.Models
{
    public class ProdutoFoto
    {
        public long Id { get; set; }
        public long ProdutoId { get; set; }
        public string Caminho { get; set; }
    }
}