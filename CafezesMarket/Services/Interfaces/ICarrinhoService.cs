using CafezesMarket.Models;
using System.Threading.Tasks;
using static CafezesMarket.Models.Enums;

namespace CafezesMarket.Services.Interfaces
{
    public interface ICarrinhoService : IBaseService
    {
        Task<bool> InserirItemCarrinhoAsync(NovoCarrinhoItem novoCarrinhoItem);
        Task RemoteItemCarrinhoAsync(long clientId, long carrinhoItemId);

    }
}
