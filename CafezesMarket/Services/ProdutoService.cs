using CafezesMarket.Infrastructure.Database.Context;
using CafezesMarket.Models;
using CafezesMarket.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafezesMarket.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private const string _prefixCache = "produto_cache";

        private readonly IMemoryCache _cache;
        private readonly TimeSpan _expirationCache;

        public ProdutoService(ILogger<ProdutoService> logger,
            DefaultContext context, IMemoryCache cache) : base(logger, context)
        {
            _cache = cache;
            _expirationCache = TimeSpan.FromSeconds(120);
        }


        public async Task<Produto> ObterAsync(long id)
        {
            var produto = await _cache.GetOrCreateAsync(
                $"{_prefixCache}_obter_{id}", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = _expirationCache;

                    return _context.Set<Produto>()
                    .Include(prod => prod.Fotos)
                    .Where(prod => prod.Ativo && prod.Id.Equals(id))
                    .AsNoTracking()
                    .SingleOrDefaultAsync();
                });

            if (produto == null)
            {
                throw new KeyNotFoundException($"Produto chave({id}) não encontrado");
            }

            return produto;
        }

        public async Task<int> CountAsync(bool comEstoque = true)
        {
            var count = await _cache.GetOrCreateAsync(
                $"{_prefixCache}_count_{comEstoque}", entry =>
                {
                    var query = _context.Set<Produto>()
                        .Where(prod => prod.Ativo);

                    if (comEstoque)
                    {
                        query = query.Where(prod => prod.Quantidade > 0);
                    }

                    return query.CountAsync();
                });

            return count;
        }

        public async Task<IList<Produto>> ObterMaisVendidosAsync(int page = 1, int pageSize = 10, bool comEstoque = true)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(page), "A págine deve ser maior ou igual a 1");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "O tamanho da página deve ser maior ou igual a 1");
            }

            var produtos = await _cache.GetOrCreateAsync(
                $"{_prefixCache}_vendidos_{page}_{pageSize}_{comEstoque}", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = _expirationCache;

                    var query = _context.Set<Produto>()
                        .Include(produto => produto.Fotos)
                        .Where(prod => prod.Ativo);

                    if (comEstoque)
                    {
                        query = query.Where(prod => prod.Quantidade > 0);
                    }

                    return query.OrderByDescending(prod =>
                            prod.PedidosItems.Where(item => item.Pedido.SituacaoId.Equals(5)).Sum(item => item.Quantidade))
                        .ThenByDescending(prod =>
                            prod.PedidosItems.Where(item => item.Pedido.SituacaoId.Equals(5)).Average(item => item.Preco))
                        .ThenByDescending(prod => prod.Preco)
                        .ThenByDescending(prod => prod.Quantidade)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .AsNoTracking()
                        .ToListAsync();
                });

            return produtos;
        }
    }
}