using System;
using System.Collections.Generic;

namespace CafezesMarket.Models
{
    public class Cliente
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime Nascimento { get; set; }
        public string Email { get; set; }

        public virtual List<Endereco> Enderecos { get; set; }
        public virtual Credencial Credencial { get; set; }
    }
}