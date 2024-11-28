using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCRM.ApiContracts.DTO.User
{
    public sealed record LoginResource(string Username, string Password);
    public sealed record UserResource(long Id,string Username,RoleDTO Role,PersonDTO Person);
    public sealed record RegisterResource(
        string Username, string Password,
        int RoleId,int PersonId,
        long Permissions
        );

    public sealed record LogOutResource(string refreshToken = null,string deviceId = null,bool isMobile = false);
    public class AuthenticatedResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public UserResource? User { get; set; }
    }
    public class TokenApiModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
