using System;
using System.Security.Cryptography;
using System.Text;

namespace CafezesMarket.Models
{
    public class Credencial
    {
        public long Id { get; set; }
        public long ClienteId { get; set; }
        public string Senha { get; set; }
        public string Salt { get; set; }
        public int Erros { get; set; }
        public DateTime? UltimoErro { get; set; }


        public bool ValidarSenha(string senha)
        {
            if (this.ProximaTentativa() > DateTime.Now)
            {
                return false;
            }

            if (this.Senha.Equals(senha))
            {
                this.Erros = 0;
                this.UltimoErro = null;

                return true;
            }

            this.Erros++;
            this.UltimoErro = DateTime.Now;

            return false;
        }

        public DateTime ProximaTentativa()
        {
            if (this.Erros >= 3)
            {
                return this.UltimoErro.GetValueOrDefault()
                    .AddSeconds(Math.Pow(2, this.Erros));
            }

            return DateTime.Now;
        }

        public void CriptografarSenha()
        {
            if (string.IsNullOrWhiteSpace(Senha))
            {
                throw new ArgumentNullException(nameof(Senha));
            }

            var data = Encoding.ASCII.GetBytes(Senha);

            using var shaM = new SHA512Managed();
            var result = shaM.ComputeHash(data);

            Senha = Convert.ToBase64String(result);
        }
    }
}