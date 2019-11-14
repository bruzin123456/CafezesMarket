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


        public async Task<Cliente> ObterAsync(long id)
        {
            var cliente = await _context.Set<Cliente>()
                .Include(cli => cli.Enderecos)
                .Where(cli => cli.Id.Equals(id))
                .SingleOrDefaultAsync();

            await _context.Entry(cliente)
                .Collection(cli => cli.Pedidos)
                .Query().OrderByDescending(pedido => pedido.Emissao)
                .Take(10)
                .AsNoTracking()
                .LoadAsync();

            return cliente;
        }

        public async Task<CadastroCliente> InserirClienteAsync(SignUp model)
        {
            var cliente = new Cliente(model);
            if (!cliente.EhValidoCpf())
            {
                return CadastroCliente.CpfInvalido;
            }
            else if (!cliente.EhValidaDataNascimento())
            {
                return CadastroCliente.DataNascimentoInvalida;
            }
            else if (string.IsNullOrWhiteSpace(model.Senha) ||
                model.Senha.Length < 5 || !model.Senha.Equals(model.ConfirmaSenha))
            {
                return CadastroCliente.SenhaInvalida;   
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
                Senha = model.Senha
            };
            cliente.Credencial.CriptografarSenha();

            await _context.Set<Cliente>().AddAsync(cliente);
            await _context.SaveChangesAsync();

            return CadastroCliente.Sucesso;
        }

    }
}
