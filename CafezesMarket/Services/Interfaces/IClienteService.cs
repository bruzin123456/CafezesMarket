using CafezesMarket.Models;
using System.Threading.Tasks;
using static CafezesMarket.Models.Enums;

namespace CafezesMarket.Services.Interfaces
{
    public interface IClienteService : IBaseService
    {
        Task<Cliente> ObterAsync(long id);
        Task<CadastroCliente> InserirClienteAsync(SignUp model);

        Task DesativarEnderecoAsync(long userId, long enderecoId);
    }
}
