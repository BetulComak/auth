using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Business.Models.Commands
{
    public record LoginCommand
    {
        /// <summary>
        /// User name of the user
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Password of the user
        /// </summary>
        /// <value></value>
        public string Password { get; set; }
    }
}
