using CafezesMarket.Models;
using System.Threading.Tasks;
using static CafezesMarket.Models.Enums;

namespace CafezesMarket.Services.Interfaces
{
    public interface IClienteService : IBaseService
    {
        Task<CadastroCliente> InserirClienteAsync(SignUp model);
    }
}
