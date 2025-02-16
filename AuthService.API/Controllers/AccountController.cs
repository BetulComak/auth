using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuthService.Business.Models.Commands;
using AuthService.Business.Contracts;
using Duende.IdentityServer.Validation;
using AuthService.Business.DTOs;


namespace AuthService.API.Controllers
{
    public class AccountController : Controller
    {
            private readonly UserManager<IdentityUser> _userManager;
            private readonly SignInManager<IdentityUser> _signInManager;
            private readonly IAccountService _accountService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IAccountService accountService)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _accountService = accountService;
            }

            // POST: api/Account/register
            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] RegisterCommand request)
            {
                await _accountService.RegisterAsync(request);
                return Ok();
            }

        // POST: api/Account/register
            [HttpPost("login")]
            public async Task<LoginResponse> Login([FromBody] LoginCommand request)
            {
                return await _accountService.LoginAsync(request);
                //return Ok();
            }
    }
}
