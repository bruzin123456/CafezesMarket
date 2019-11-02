using CafezesMarket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CafezesMarket.Services.Interfaces
{
    public interface IProdutoService : IBaseService
    {
        Task<Produto> ObterAsync(long id);
        Task<int> CountAsync(bool comEstoque = true);
        Task<IList<Produto>> ObterMaisVendidosAsync(int page = 1, int pageSize = 10, bool comEstoque = true);
    }
}
