using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CafezesMarket.Models;
using CafezesMarket.Services.Interfaces;
using System;

namespace CafezesMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IProdutoService _produtoService;

        public HomeController(ILogger<HomeController> logger,
            IProdutoService produtoService)
        {
            _logger = logger;
            _produtoService = produtoService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var tops = await _produtoService
                    .ObterMaisVendidosAsync(pageSize: 4);

                return View(model: tops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Home - Index - Erro");

                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
