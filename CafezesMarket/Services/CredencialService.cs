using CafezesMarket.Infrastructure.Database.Context;
using CafezesMarket.Models;
using CafezesMarket.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CafezesMarket.Services
{
    public class CredencialService : BaseService, ICredencialService
    {
        public CredencialService(ILogger<CredencialService> logger,
            DefaultContext context) : base(logger, context)

        {

        }

        public async Task<Login> ValidarLoginAsync(Login login)
        {
            var cliente = await _context.Set<Cliente>()
                .Include(client => client.Credencial)
                .Where(client => client.Email.Equals(login.Email))
                .SingleOrDefaultAsync();

            if (cliente == null)
            {
                login.Mensagem = "Email;E-mail não cadastrado.";
            }
            else if (cliente.Credencial == null)
            {
                login.Mensagem = "Senha;Senha não cadastrada.";
            }
            else if (cliente.Credencial.ValidarSenha(login.Senha))
            {
                login.Valido = true;
                login.Principal = this.CriarPermissao(cliente);
            }
            else
            {
                if (cliente.Credencial.Erros >= 3)
                {
                    login.Mensagem = $"Senha;Tentativa bloqueada, tente de novo após {cliente.Credencial.ProximaTentativa()}";
                }
                else
                {
                    login.Mensagem = "Senha;Senha incorreta...";
                }
            }
            await _context.SaveChangesAsync();

            return login;
        }

        private ClaimsPrincipal CriarPermissao(Cliente cliente)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, cliente.Nome));
            identity.AddClaim(new Claim(ClaimTypes.Email, cliente.Email));
            identity.AddClaim(new Claim(ClaimTypes.DateOfBirth, cliente.Nascimento.ToShortDateString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, "cliente"));

            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
