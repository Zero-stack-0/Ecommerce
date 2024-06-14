using System.Security.Claims;
using Service.Dto;
using Service.Interface;

namespace Webservice.Helper
{
    public class CookieUserDetailsHandler
    {
        private readonly IAccountService accountService;
        public CookieUserDetailsHandler(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task<UserResponse?> GetUserDetail(ClaimsIdentity? claimsIdentity)
        {
            if (claimsIdentity is not null && !claimsIdentity.Claims.Any())
            {
                return null;
            }

            var requestorEmailId = claimsIdentity.Claims.FirstOrDefault(e => e.Type.Contains("emailaddress")).Value;

            try
            {
                return await accountService.GetUserProfile(requestorEmailId);
            }
            catch
            {
                return null;
            }
        }
    }
}