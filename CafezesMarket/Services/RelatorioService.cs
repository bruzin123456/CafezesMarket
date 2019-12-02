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
    public class RelatorioService : BaseService, IRelatorioService
    {
        public RelatorioService(ILogger<RelatorioService> logger,
            DefaultContext context) : base(logger, context)
        {
        }

        public async Task<IReadOnlyList<object>> ProdutosEstoqueAsync(int page, int pageSize)
        {
            if (page < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(page), "Deve ser maior que zero");
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Deve ser maior que zero");
            }

            var total = await _context.Set<Produto>()
                .Where(produto => produto.Ativo && produto.Quantidade > 0)
                .AsNoTracking()
                .CountAsync();

            if (((page - 1) * pageSize) > total)
            {
                throw new ArgumentOutOfRangeException(nameof(page), "Pagina solicita não existe");
            }

            var produtos = await _context.Set<Produto>()
                .Where(produto => produto.Ativo && produto.Quantidade > 0)
                .OrderByDescending(produto => produto.Quantidade)
                    .ThenBy(produto => produto.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(produto => new
                {
                    produto.Id,
                    produto.Titulo,
                    produto.Quantidade
                })
                .AsNoTracking()
                .ToListAsync();

            return produtos;
        }

        public async Task<IReadOnlyList<object>> ProdutosVendidosAsync(int page, int pageSize, int dias)
        {
            if (page < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(page), "Deve ser maior que zero");
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Deve ser maior que zero");
            }
            if (dias < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(dias), "Deve ser maior que zero");
            }

            var dataApos = DateTime.Today.AddDays((-1) * dias);

            var total = await _context.Set<Pedido>()
                .Where(pedido => pedido.Emissao >= dataApos && pedido.SituacaoId.Equals(5))
                .SelectMany(pedido => pedido.Itens)
                .GroupBy(itens => itens.ProdutoId)
                .Select(itens => itens.Key)
                .CountAsync();

            if (((page - 1) * pageSize) > total)
            {
                throw new ArgumentOutOfRangeException(nameof(page), "Pagina solicita não existe");
            }

            var produtos = await _context.Set<Pedido>()
                .Where(pedido => pedido.Emissao >= dataApos && pedido.SituacaoId.Equals(5))
                .SelectMany(pedido => pedido.Itens)
                .GroupBy(itens => new 
                {
                    itens.Produto.Id,
                    itens.Produto.Titulo
                })
                .Select(group => new
                {
                    group.Key.Id,
                    group.Key.Titulo,
                    Quantidade = group.Sum(itens => itens.Quantidade)
                })
                .OrderByDescending(group => group.Quantidade)
                    .ThenBy(group => group.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return produtos;
        }
    }
}
