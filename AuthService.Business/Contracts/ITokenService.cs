using Duende.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Business.Contracts
{
    public interface ITokenService
    {
        Task<string> CreateSecurityTokenAsync(Token request);

    }
}
