using AuthService.Business.Contracts;
using AuthService.Business.DTOs;
using AuthService.Business.Models.Commands;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Duende.IdentityServer.Validation;
using Duende.IdentityModel;
using static Duende.IdentityServer.Models.IdentityResources;
using Duende.IdentityServer.Models;
using Microsoft.VisualBasic;
using Duende.IdentityServer.Services;


namespace AuthService.Business.Services
{
    public class AccountService : IAccountService
    {
        readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AuthService.Business.Contracts.ITokenService _tokenService;

        public AccountService(UserManager<IdentityUser> userManager,
            AuthService.Business.Contracts.ITokenService tokenService,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterCommand request)
        {
            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new Exception("Registration failed.");
            }

            return new RegisterResponse();
        }

        public async Task<LoginResponse> LoginAsync(LoginCommand request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(request.Email);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                var claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Issuer, "http://localhost:5229"),
                        new Claim(JwtClaimTypes.Email, user.Email),
                        new Claim(JwtClaimTypes.Name, user.UserName),
                        new Claim(JwtClaimTypes.Audience, "file-service") // this can be an enum. For other services, the audience is taken as a parameter and set
                    };

                var tokenResult = await _tokenService.CreateSecurityTokenAsync(new Token
                {
                    CreationTime = DateTime.UtcNow,
                    Lifetime = 3600,
                    Issuer = "http://localhost:5229",
                    ClientId = "file-service",
                    AccessTokenType = AccessTokenType.Jwt,
                    Claims = claims
                });

                return new LoginResponse
                {
                    AccessToken = tokenResult
                };
            }
            else if (result.IsLockedOut)
            {
                throw new Exception("Account locked. Please try again later.");
            }
            else if (result.IsNotAllowed)
            {
                throw new Exception("Login not allowed. Please confirm your email.");
            }
            else
            {
                throw new Exception("Invalid username or password.");
            }
        }
    }
}
