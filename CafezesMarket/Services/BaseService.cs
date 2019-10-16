using CafezesMarket.Infrastructure.Database.Context;
using CafezesMarket.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CafezesMarket.Services
{
    public abstract class BaseService : IBaseService
    {
        internal readonly ILogger _logger;
        internal readonly DefaultContext _context;

        public BaseService(ILogger logger, DefaultContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}