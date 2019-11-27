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
                .Where(cli => cli.Id.Equals(id))
                .SingleOrDefaultAsync();

            if (cliente != null)
            {
                await _context.Entry(cliente)
                    .Collection(cli => cli.Pedidos)
                    .Query()
                    .Include(pedido => pedido.Situacao)
                    .OrderByDescending(pedido => pedido.Emissao)
                    .Take(10)
                    .LoadAsync();

                await _context.Entry(cliente)
                    .Collection(cli => cli.Enderecos)
                    .Query()
                    .Include(endereco => endereco.Estado)
                    .Where(endereco => endereco.Ativo)
                    .LoadAsync();
            }

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
                .AnyAsync();

            if (clienteExistente)
            {
                return CadastroCliente.EmailJaExiste;
            }

            clienteExistente = await _context.Set<Cliente>()
                .Where(cli => cli.Cpf.Equals(cliente.Cpf))
                .AsNoTracking()
                .AnyAsync();

            if (clienteExistente)
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


        public async Task<CadastroEndereco> InserirEnderecoAsync(NovoEndereco novoEndereco)
        {
            if (novoEndereco == null)
            {
                throw new ArgumentNullException(nameof(novoEndereco));
            }

            var estado = await _context.Set<Estado>()
                .Where(est => est.Sigla.Equals(novoEndereco.Estado.ToUpper()))
                .SingleOrDefaultAsync();

            if (estado == null)
            {
                return CadastroEndereco.EstadoInvalido;
            }

            var endereco = new Endereco(novoEndereco)
            {
                Estado = estado
            };

            await _context.Set<Endereco>()
                .AddAsync(endereco);
            await _context.SaveChangesAsync();

            return CadastroEndereco.Sucesso;
        }

        public async Task DesativarEnderecoAsync(long userId, long enderecoId)
        {
            var endereco = await _context.Set<Endereco>()
                .Where(ende => ende.Id.Equals(enderecoId) &&
                    ende.ClienteId.Equals(userId) && ende.Ativo)
                .SingleOrDefaultAsync();

            if (endereco == null)
            {
                throw new ArgumentException("Endereço não encontrado ou já inativado",
                    nameof(enderecoId));
            }

            endereco.Ativo = false;

            await _context.SaveChangesAsync();
        }
    }
}
