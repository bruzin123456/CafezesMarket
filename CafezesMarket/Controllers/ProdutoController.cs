using System;
using System.Threading.Tasks;
using CafezesMarket.Models;
using CafezesMarket.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace CafezesMarket.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ILogger _logger;
        private readonly IProdutoService _produtoService;

        public ProdutoController(ILogger<ProdutoController> logger,
            IProdutoService produtoService)
        {
            _logger = logger;
            _produtoService = produtoService;
        }

        [HttpGet]
        [Route("Produto")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            try
            {
                var produtos = await _produtoService
                    .ObterMaisVendidosAsync(page, pageSize, false);

                if (produtos.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }

                var totalProdutos = await _produtoService.CountAsync(false);

                var model = new StaticPagedList<Produto>(produtos, page, pageSize, totalProdutos);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Produto - Index - Erro");

                throw;
            }
        }

        [HttpGet]
        [Route("Produto/{id}")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> Detalhe([FromRoute] long id)
        {
            try
            {
                var model = await _produtoService
                    .ObterAsync(id);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Produto - Detalhe - Erro");

                throw;
            }
        }
    }
}