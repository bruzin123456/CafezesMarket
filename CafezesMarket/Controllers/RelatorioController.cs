using System;
using System.Net.Mime;
using System.Threading.Tasks;
using CafezesMarket.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CafezesMarket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class RelatorioController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IRelatorioService _relatorioService;

        public RelatorioController(ILogger<RelatorioController> logger,
            IRelatorioService relatorioService)
        {
            _logger = logger;
            _relatorioService = relatorioService;
        }


        [HttpGet]
        [Route("Produto/Estoque")]
        public async Task<IActionResult> EstoqueProdutos(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var resultado = await _relatorioService
                    .ProdutosEstoqueAsync(page, pageSize);

                return new JsonResult(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Relatorio - EstoqueProdutos - Erro");

                throw;
            }
        }


        [HttpGet]
        [Route("Produto/Vendas")]
        public async Task<IActionResult> VendasProdutos(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] int dias = 90)
        {
            try
            {
                var resultado = await _relatorioService
                    .ProdutosVendidosAsync(page, pageSize, dias);

                return new JsonResult(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Relatorio - VendasProdutos - Erro");

                throw;
            }
        }
    }
}