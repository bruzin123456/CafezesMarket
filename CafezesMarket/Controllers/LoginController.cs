using CafezesMarket.Models;
using CafezesMarket.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using static CafezesMarket.Models.Enums;

namespace CafezesMarket.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger _logger;
        private readonly IClienteService _clienteService;
        private readonly ICredencialService _credencialService;

        public LoginController(ILogger<LoginController> logger,
            IClienteService clienteService,
            ICredencialService credencialService)
        {
            _logger = logger;
            _clienteService = clienteService;
            _credencialService = credencialService;
        }

        [HttpGet]
        [Route("Login")]
        [Route("Login/SignIn")]
        public IActionResult SignIn(string returnUrl = null)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }

                var model = new Login()
                {
                    ReturnUrl = returnUrl
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login - SignIn Get - Erro");

                throw;
            }
        }

        [HttpPost]
        [Route("Login")]
        [Route("Login/SignIn")]
        public async Task<IActionResult> SignIn(Login model)
        {
            try
            {
                var login = await _credencialService
                    .ValidarLoginAsync(model);

                if (login.Valido)
                {
                    _logger.LogInformation($"Login - SignIn - Sucesso - User {User.Identity.Name}");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            login.Principal);

                    if (!string.IsNullOrWhiteSpace(login.ReturnUrl) && Url.IsLocalUrl(login.ReturnUrl))
                    {
                        return Redirect(login.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                _logger.LogInformation($"Login - SignIn - Falhou - User {User.Identity.Name}");

                var mensanges = login.Mensagem?.Split(';',
                    StringSplitOptions.RemoveEmptyEntries);

                ModelState.AddModelError(mensanges[0], mensanges[1]);

                return View(login);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login - SignIn Post - Erro");

                return StatusCode((int)HttpStatusCode.InternalServerError,
                    "Desculpe, ocorreu um erro.");
            }
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> SignOut()
        {
            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    _logger.LogInformation($"Login - SignOut - User {User.Identity.Name}");
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login - SignOut - Erro");

                throw;
            }
        }


        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            try
            {
                await Task.Delay(100);
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login - SignUp Get - Erro");

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Cliente model)
        {
            try
            {
                var resposta = await _clienteService
                    .InserirClienteAsync(model);

                switch (resposta)
                {
                    case CadastroCliente.CpfInvalido:
                        ModelState.AddModelError("Cpf", "Cpf inválido, digite somente números, sem pontos ou traços.");
                        break;

                    case CadastroCliente.DataNascimentoInvalida:
                        ModelState.AddModelError("Nascimento", "Idade mínima de 12 anos.");
                        break;

                    case CadastroCliente.EmailJaExiste:
                        ModelState.AddModelError("Email", "E-mail já cadastrado. Verifique...");
                        break;

                    case CadastroCliente.CpfJaExiste:
                        ModelState.AddModelError("Cpf", "Cpf já cadastrado. Verifique...");
                        break;

                    case CadastroCliente.ErroSistema:
                        ModelState.AddModelError("Nome", "Desculpe ocorreu um erro...");
                        break;

                    case CadastroCliente.Sucesso:
                        ModelState.Clear();
                        ViewBag.Mensagem = "Eba. Cadastro realizado com sucesso!!!";
                        break;

                    default:
                        throw new ArgumentException("Resposta inválida", nameof(resposta));
                };

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login - SignUp Post - Erro");

                return StatusCode((int)HttpStatusCode.InternalServerError,
                    "Desculpe, ocorreu um erro.");
            }
        }
    }
}