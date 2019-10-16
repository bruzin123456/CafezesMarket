using System;
using System.Threading.Tasks;
using CafezesMarket.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
        {
            try
            {
                var model = await _produtoService
                    .ObterMaisVendidosAsync(page, pageSize, false);

                if (model.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }

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