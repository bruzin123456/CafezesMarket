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

        public virtual Credencial Credencial { get; set; }
        public virtual List<Pedido> Pedidos { get; set; }
        public virtual List<Endereco> Enderecos { get; set; }

        public Cliente()
        {
            
        }

        public Cliente(SignUp model)
        {
            this.Nome = model.Nome;
            this.Cpf = model.Cpf;
            this.Nascimento = model.Nascimento;
            this.Email = model.Email;
        }



        public bool EhValidoCpf()
        {
            if (long.TryParse(Cpf, out long result) && result > 0
                && Cpf.Length >= 6 && Cpf.Length <= 11)
            {
                return true;
            }

            return false;
        }

        public bool EhValidaDataNascimento()
        {
            if (Nascimento.Date.AddYears(12) <= DateTime.Now.Date)
            {
                return true;
            }

            return false;
        }
    }
}