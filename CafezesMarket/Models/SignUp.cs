using System;

namespace CafezesMarket.Models
{
    public class SignUp
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime Nascimento { get; set; }
        public string Email { get; set; }
        public string Senha { get;set; }
        public string ConfirmaSenha { get; set; }
    }
}