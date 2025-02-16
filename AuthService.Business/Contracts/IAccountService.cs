using AuthService.Business.DTOs;
using AuthService.Business.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Business.Contracts
{
    public interface IAccountService
    {
        Task<RegisterResponse> RegisterAsync(RegisterCommand request);
        Task<LoginResponse> LoginAsync(LoginCommand request);

    }
}
