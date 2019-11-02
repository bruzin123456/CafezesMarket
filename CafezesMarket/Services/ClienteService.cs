using CafezesMarket.Infrastructure.Database.Context;
using CafezesMarket.Models;
using CafezesMarket.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CafezesMarket.Models.Enums;

namespace CafezesMarket.Services
{
    public class ClienteService : BaseService, IClienteService
    {
        public ClienteService(ILogger<ClienteService> logger,
            DefaultContext context) : base(logger, context)
        {

        }

        public async Task<CadastroCliente> InserirClienteAsync(Cliente cliente)
        {
            if (!cliente.EhValidoCpf())
            {
                return CadastroCliente.CpfInvalido;
            }
            else if (!cliente.EhValidaDataNascimento())
            {
                return CadastroCliente.DataNascimentoInvalida;
            }

            var clienteExistente = await _context.Set<Cliente>()
                .Where(cli => cli.Email.Equals(cliente.Email))
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (clienteExistente != null)
            {
                return CadastroCliente.EmailJaExiste;
            }

            clienteExistente = await _context.Set<Cliente>()
                .Where(cli => cli.Cpf.Equals(cliente.Cpf))
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (clienteExistente != null)
            {
                return CadastroCliente.CpfJaExiste;
            }

            cliente.Credencial = new Credencial()
            {
                Senha = "123456"
            };
            cliente.Credencial.CriptografarSenha();

            await _context.Set<Cliente>().AddAsync(cliente);
            await _context.SaveChangesAsync();

            return CadastroCliente.Sucesso;
        }
    }
}
