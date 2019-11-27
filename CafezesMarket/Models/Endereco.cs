using System;
using System.ComponentModel.DataAnnotations;

namespace CafezesMarket.Models
{
    public class Endereco
    {
        public long Id { get; set; }
        public long ClienteId { get; set; }
        public int EstadoId { get; set; }
        public string Apelido { get; set; }
        public string Logradouro { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public bool Ativo { get; set; }


        public virtual Cliente Cliente { get; set; }
        public virtual Estado Estado { get; set; }

        public Endereco()
        {

        }

        public Endereco(NovoEndereco novoEndereco)
        {
            if (novoEndereco == null)
            {
                throw new ArgumentNullException(nameof(novoEndereco));
            }

            this.ClienteId = novoEndereco.ClienteId;

            this.Apelido = novoEndereco.Apelido.Trim().ToLower();
            this.Logradouro = novoEndereco.Logradouro.Trim();
            this.Numero = novoEndereco.Numero;

            if (string.IsNullOrWhiteSpace(novoEndereco.Complemento))
            {
                this.Complemento = null;
            }
            else
            {
                this.Complemento = novoEndereco.Complemento.Trim().ToLower();
            }

            this.Cidade = novoEndereco.Cidade.Trim().ToLower();

            Ativo = true;
        }

        public string ToExibicao()
        {
            var texto = string.Concat(
                this.Logradouro,
                ", ",
                this.Numero.ToString(),
                " - ",
                this.Cidade,
                " - ",
                this.Estado?.Sigla);

            return texto;
        }
    }
}