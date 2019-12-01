using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace CafezesMarket.Models
{
    public class NovoCarrinhoItem
    {

        [BindNever]
        [JsonIgnore]
        public long ClienteId { get; set; }

        [JsonPropertyName("produto")]
        public long ProdutoId { get; set; }

        [JsonPropertyName("quantidade")]
        public int Quantidade { get; set; }
    }
}
