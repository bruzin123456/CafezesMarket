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
        [Route("Cliente/Endereco")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> InserirEndereco([FromBody] NovoEndereco endereco)
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
                    endereco.ClienteId = clienteId;

                    var resultado = await _clienteService
                        .InserirEnderecoAsync(endereco);

                    switch (resultado)
                    {
                        case CadastroEndereco.EstadoInvalido:
                            _logger.LogInformation($"Cliente - InserirEndereco - Erro - Estado inválido ClienteId '{clienteId}', Estado '{endereco.Estado}'");

                            return StatusCode((int)HttpStatusCode.BadRequest,
                                Json("Estado inválido"));

                        default:
                            _logger.LogInformation($"Cliente - InserirEndereco - Sucesso - ClienteId '{clienteId}'");

                            return StatusCode((int)HttpStatusCode.OK,
                                Json("Endereço cadastrado!"));
                    }
                }
                else
                {
                    _logger.LogWarning($"Cliente - InserirEndereco - Erro - ClienteId não numérico '{claimId}'");

                    return StatusCode((int)HttpStatusCode.Unauthorized);
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
        [ValidateAntiForgeryToken]
        [Route("Cliente/Endereco/{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RemoverEndereco([FromRoute] long id)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarning($"Cliente - RemoverEndereco - Erro - Id menor que zero '{id}'");

                    return StatusCode((int)HttpStatusCode.BadRequest,
                        Json("Endereço id inválido"));
                }

                var claimId = User?.FindFirst(ClaimTypes.PrimarySid)
                    ?.Value;

                if (long.TryParse(claimId, out long clienteId))
                {
                    await _clienteService.DesativarEnderecoAsync(clienteId, id);
                    _logger.LogWarning($"Cliente - RemoverEndereco - Sucesso - ClienteId '{clienteId}', EnderecoId '{id}'");

                    return StatusCode((int)HttpStatusCode.OK,
                        Json("Endereço removido!"));
                }
                else
                {
                    _logger.LogWarning($"Cliente - RemoverEndereco - Erro - ClienteId não numérico '{claimId}'");

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