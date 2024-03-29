﻿using CafezesMarket.Models;
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
                    _logger.LogInformation($"Login - SignIn - Sucesso - User {model.Email}");

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

                _logger.LogInformation($"Login - SignIn - Falhou - User {model?.Email}");

                var mensanges = login.Mensagem?.Split(';',
                    StringSplitOptions.RemoveEmptyEntries);

                if (mensanges?.Length > 0)
                {
                    ModelState.AddModelError(mensanges[0], mensanges[1]);
                }

                return View(login);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login - SignIn Post - Erro");

                throw;
            }
        }


        [AcceptVerbs("Get", "Post")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
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
        [ResponseCache(Duration = 120)]
        public IActionResult SignUp()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login - SignUp Get - Erro");

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUp model)
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

                    case CadastroCliente.SenhaInvalida:
                        ModelState.AddModelError("Senha", "Algo de errado com a senha... :(");
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