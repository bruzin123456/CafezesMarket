using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CafezesMarket.Models;
using CafezesMarket.Services.Interfaces;

namespace CafezesMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProdutoService _produtoService;

        public HomeController(ILogger<HomeController> logger,
            IProdutoService produtoService)
        {
            _logger = logger;
            _produtoService = produtoService;
        }

        [ResponseCache(Duration = 120)]
        public async Task<IActionResult> Index()
        {
            var tops = await _produtoService
                .ObterMaisVendidosAsync(pageSize: 4);

            return View(model: tops);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
