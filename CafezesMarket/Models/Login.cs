using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CafezesMarket.Models
{
    public class Login
    {
        public Login()
        {

        }

        public Login(string email, string senha)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }
            if (string.IsNullOrWhiteSpace(senha))
            {
                throw new ArgumentNullException(nameof(senha));
            }

            ;

            this.Email = email;
            this.Senha = senha;
        }

        public string Email { get; set; }


        private string _senha;
        public string Senha
        {
            get => _senha;

            set => _senha = this.CriptografarSenha(value);
        }


        public string ReturnUrl { get; set; }

        public bool Valido { get; set; } = false;
        public string Mensagem { get; set; }
        public ClaimsPrincipal Principal { get; set; }


        private string CriptografarSenha(string senha)
        {
            var data = Encoding.ASCII.GetBytes(senha);

            using var shaM = new SHA512Managed();
            var result = shaM.ComputeHash(data);

            var finalResult = Convert.ToBase64String(result);
            return finalResult;
        }
    }
}
