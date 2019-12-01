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
    public class CarrinhoService : BaseService, ICarrinhoService
    {

        public CarrinhoService(ILogger<CarrinhoService> logger,
     DefaultContext context) : base(logger, context)
        {

        }

        public async Task<bool> InserirItemCarrinhoAsync(NovoCarrinhoItem novoCarrinhoItem)
        {
            if (novoCarrinhoItem == null)
            {
                throw new ArgumentNullException(nameof(novoCarrinhoItem));
            }

            var carrinhoItem = await _context.Set<CarrinhoItem>()
                .Where(cItem => cItem.ClienteId.Equals(novoCarrinhoItem.ClienteId) && cItem.ProdutoId.Equals(novoCarrinhoItem.ProdutoId))
                .SingleOrDefaultAsync();
            if(carrinhoItem == null)
            {
                carrinhoItem = new CarrinhoItem(novoCarrinhoItem);
                await _context.Set<CarrinhoItem>()
                .AddAsync(carrinhoItem);
            }
            else
            {
                carrinhoItem.Quantidade += novoCarrinhoItem.Quantidade;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RemoteItemCarrinhoAsync(long clientId, long carrinhoItemId)
        {


            var carrinhoItem = await _context.Set<CarrinhoItem>()
         .Where(cItem => cItem.Id.Equals(carrinhoItemId))
         .SingleOrDefaultAsync();

            if (carrinhoItem == null)
            {
                throw new ArgumentException("Item do carrinho não encontrado",
                    nameof(carrinhoItem));
            }

            if (!carrinhoItem.ClienteId.Equals(clientId))
            {
                throw new ArgumentException("Item do carrinho de outro usuário",
                    nameof(carrinhoItem));
            }

            _context.Set<CarrinhoItem>().Remove(carrinhoItem);
            await _context.SaveChangesAsync();
        }
    }
}
