namespace CafezesMarket.Models
{
    public class Credencial
    {
        public long Id { get; set; }
        public long ClienteId { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Salt { get; set; }
    }
}