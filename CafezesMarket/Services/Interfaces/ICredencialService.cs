using CafezesMarket.Models;
using System.Threading.Tasks;

namespace CafezesMarket.Services.Interfaces
{
    public interface ICredencialService : IBaseService
    {
        Task<Login> ValidarLoginAsync(Login login);
    }
}
