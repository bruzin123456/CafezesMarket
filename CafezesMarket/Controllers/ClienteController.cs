using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CafezesMarket.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CafezesMarket.Controllers
{
    [Authorize(Roles = "cliente")]
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IClienteService _clienteService;

        public ClienteController(ILogger<ClienteController> logger,
            IClienteService clienteService)
        {
            _logger = logger;
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var claimId = User.FindFirst(ClaimTypes.PrimarySid)
                    ?.Value;

                if (long.TryParse(claimId, out long id))
                {
                    var model = await _clienteService.ObterAsync(id);

                    return View(model);
                }
                else
                {
                    throw new InvalidCastException("User PrimarySid inválida");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cliente - Index - Erro");

                throw;
            }
        }
    }
}