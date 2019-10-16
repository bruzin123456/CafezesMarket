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
                .Where(prod => prod.Id.Equals(id))
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (produto == null)
            {
                throw new KeyNotFoundException($"Produto chave({id}) não encontrado");
            }

            return produto;
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

            var query = _context.Set<PedidoItem>()
                .Where(item => item.Pedido.Situacao.Id != 1);

            if (comEstoque)
            {
                query = query.Where(item => item.Produto.Quantidade > 0);
            }
                    
            var produtosIds = await query.GroupBy(item => item.ProdutoId)
                .Select(group => new
                {
                    ProdutoId = group.Key,
                    TotalVendido = group.Sum(item => item.Quantidade),
                    PrecoMedio = group.Average(item => item.Preco)
                })
                .OrderByDescending(group => group.TotalVendido)
                    .ThenByDescending(group => group.PrecoMedio)
                .Select(group => group.ProdutoId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();

            var produtos = await _context.Set<Produto>()
                .Include(produto => produto.Fotos)
                .Where(produto => produtosIds.Contains(produto.Id))
                .AsNoTracking()
                .ToListAsync();

            return produtos;
        }
    }
}