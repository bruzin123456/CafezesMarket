using CafezesMarket.Infrastructure.Database.Context;
using CafezesMarket.Models;
using CafezesMarket.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafezesMarket.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        public ProdutoService(ILogger<ProdutoService> logger,
            DefaultContext context) : base (logger, context)
        {

        }


        public async Task<Produto> ObterAsync(long id)
        {
            var produto = await _context.Set<Produto>()
                .Include(prod => prod.Fotos)
                .Where(prod => prod.Ativo && prod.Id.Equals(id))
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (produto == null)
            {
                throw new KeyNotFoundException($"Produto chave({id}) não encontrado");
            }

            return produto;
        }

        public async Task<int> CountAsync(bool comEstoque = true)
        {
            var query = _context.Set<Produto>()
                .Where(prod => prod.Ativo);

            if (comEstoque)
            {
                query = query.Where(prod => prod.Quantidade > 0);
            }

            var count = await query.CountAsync();

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

            var query = _context.Set<Produto>()
                .Include(produto => produto.Fotos)
                .Where(prod => prod.Ativo);

            if (comEstoque)
            {
                query = query.Where(prod => prod.Quantidade > 0);
            }

            var produtos = await query
                .OrderByDescending(prod =>
                    prod.PedidosItems.Where(item => item.Pedido.SituacaoId.Equals(5)).Sum(item => item.Quantidade))
                .ThenByDescending(prod =>
                    prod.PedidosItems.Where(item => item.Pedido.SituacaoId.Equals(5)).Average(item => item.Preco))
                .ThenByDescending(prod => prod.Preco)
                .ThenByDescending(prod => prod.Quantidade)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return produtos;
        }
    }
}