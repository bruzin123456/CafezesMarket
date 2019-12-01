using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using CafezesMarket.Models;
using CafezesMarket.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static CafezesMarket.Models.Enums;

namespace CafezesMarket.Controllers
{

    [Authorize(Roles = "cliente")]
    public class CarrinhoController : Controller
    {

        private readonly ILogger<CarrinhoController> _logger;
        private readonly IClienteService _clienteService;
        private readonly ICarrinhoService _carrinhoService;


        public CarrinhoController(ILogger<CarrinhoController> logger,
               IClienteService clienteService,
               ICarrinhoService carrinhoService)
        {
            _logger = logger;
            _clienteService = clienteService;
            _carrinhoService = carrinhoService;
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
        [Route("Carrinho/Adicionar")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AdicionarAoCaorrinho([FromBody] NovoCarrinhoItem carrinhoItem)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Select(model => model.Value.Errors)
                        .Where(model => model.Count > 0)
                        .SelectMany(errors => errors.Select(erro => erro.ErrorMessage));

                    return StatusCode((int)HttpStatusCode.BadRequest,
                        Json(errors));
                }

                var claimId = User
                    ?.FindFirst(ClaimTypes.PrimarySid)
                    ?.Value;

                if (long.TryParse(claimId, out long clienteId))
                {
                    carrinhoItem.ClienteId = clienteId;

                    var resultado = await _carrinhoService
                        .InserirItemCarrinhoAsync(carrinhoItem);

                    if (resultado)
                    {
                        _logger.LogInformation($"Cliente - AdicionarAoCarrinho - Sucesso - ClienteId '{clienteId}'");
                        return StatusCode((int)HttpStatusCode.OK,
                            Json("Item adicionado ao carrinho!"));
                    } else
                    {
                        _logger.LogInformation($"Cliente - AdicionarAoCarrinho - Erro - Estado inválido carrinho '{clienteId}'");

                        return StatusCode((int)HttpStatusCode.BadRequest,
                            Json("Falha ao adicionar ao carrinho"));
                    }
                }
                else
                {
                    _logger.LogWarning($"Cliente - AdicionarAoCarrinho - Erro - ClienteId não numérico '{claimId}'");
                    return StatusCode((int)HttpStatusCode.Unauthorized, Json("Precisa estar logado!"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cliente - AdicionarEndereco - Erro");

                return StatusCode((int)HttpStatusCode.InternalServerError,
                    Json("Ocorreu um erro ao processar a requisição."));
            }
        }

        [HttpDelete]
        [Route("Carrinho/Remover/{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RemoveItemCarrinho([FromRoute] long id)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarning($"Cliente - RemoveItemCarrinho - Erro - Id menor que zero '{id}'");

                    return StatusCode((int)HttpStatusCode.BadRequest,
                        Json("Endereço id inválido"));
                }

                var claimId = User?.FindFirst(ClaimTypes.PrimarySid)
                    ?.Value;

                if (long.TryParse(claimId, out long clienteId))
                {
                    await _carrinhoService.RemoteItemCarrinhoAsync(clienteId, id);
                    _logger.LogWarning($"Cliente - RemoveItemCarrinho - Sucesso - ClienteId '{clienteId}', itemId '{id}'");

                    return StatusCode((int)HttpStatusCode.OK,
                        Json("Item removido do carrinho!"));
                }
                else
                {
                    _logger.LogWarning($"Cliente - RemoveItemCarrinho - Erro - ClienteId não numérico '{claimId}'");

                    return StatusCode((int)HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cliente - RemoveItemCarrinho - Erro - Id {id}");

                return StatusCode((int)HttpStatusCode.InternalServerError,
                    Json("Ocorreu um erro ao processar a requisição."));
            }
        }

    }
}
