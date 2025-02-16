using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

namespace AuthService.Business.Services
{
    public class TokenService : AuthService.Business.Contracts.ITokenService
    {
        private readonly ITokenCreationService _tokenCreationService;

        public TokenService(ITokenCreationService tokenCreationService)
        {
            _tokenCreationService = tokenCreationService;
        }

        public async Task<string> CreateSecurityTokenAsync(Token request)
        {
            var token = await _tokenCreationService.CreateTokenAsync(request);
            return token;
        }

       
    }
}
