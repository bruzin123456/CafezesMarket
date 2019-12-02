using System.Collections.Generic;
using System.Threading.Tasks;

namespace CafezesMarket.Services.Interfaces
{
    public interface IRelatorioService
    {
        Task<IReadOnlyList<object>> ProdutosEstoqueAsync(int page, int pageSize);
        Task<IReadOnlyList<object>> ProdutosVendidosAsync(int page, int pageSize, int dias);
    }
}
