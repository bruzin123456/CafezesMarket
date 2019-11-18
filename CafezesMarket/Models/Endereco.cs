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
        public Estado Estado { get; set; }

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