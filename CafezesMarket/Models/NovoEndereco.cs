using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CafezesMarket.Models
{
    public class NovoEndereco
    {
        [BindNever]
        [JsonIgnore]
        public long ClienteId { get; set; }

        [JsonPropertyName("apelido")]
        [Required(ErrorMessage = "o apelido é obrigatório.")]
        [StringLength(128, ErrorMessage = "o apelido deve conter entre 3 e 128 caracteres.", MinimumLength = 3)]
        public string Apelido { get; set; }

        [JsonPropertyName("logradouro")]
        [Required(ErrorMessage = "o logradouro é obrigatório.")]
        [StringLength(512, ErrorMessage = "o logradouro deve conter entre 3 e 512 caracteres.", MinimumLength = 3)]
        public string Logradouro { get; set; }

        [JsonPropertyName("numero")]
        [Required(ErrorMessage = "o número é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "o número deve ser maior que zero.")]
        public int Numero { get; set; }

        [JsonPropertyName("complemento")]
        [MaxLength(1024, ErrorMessage = "o complemento deve conter no máximo 1024 caracteres.")]
        public string Complemento { get; set; }

        [JsonPropertyName("cidade")]
        [Required(ErrorMessage = "a cidade é obrigatória.")]
        [StringLength(512, ErrorMessage = "a cidade deve conter entre 3 e 512 caracteres.", MinimumLength = 3)]
        public string Cidade { get; set; }

        [JsonPropertyName("estado")]
        [Required(ErrorMessage = "o estado é obrigatório.")]
        [StringLength(2, ErrorMessage = "preencha o estado com a sigla, exemplo 'SP'", MinimumLength = 2)]
        public string Estado { get; set; }
    }
}
