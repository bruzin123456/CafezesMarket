using System;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using CafezesMarket.Services.Interfaces;
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
                var claimId = User?.FindFirst(ClaimTypes.PrimarySid)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Cliente/RemoverEndereco/{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RemoverEndereco([FromRoute] long id)
        {
            try
            {
                if (id < 0)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest,
                        Json("Endereço id inválido"));
                }

                var claimId = User?.FindFirst(ClaimTypes.PrimarySid)
                    ?.Value;

                if (long.TryParse(claimId, out long userId))
                {
                    await _clienteService.DesativarEnderecoAsync(userId, id);

                    return StatusCode((int)HttpStatusCode.OK,
                        Json("Endereço removido!"));
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cliente - RemoverEndereco - Erro - Id {id}");

                return StatusCode((int)HttpStatusCode.InternalServerError,
                    Json("Ocorreu um erro ao processar a requisição."));
            }
        }
    }
}