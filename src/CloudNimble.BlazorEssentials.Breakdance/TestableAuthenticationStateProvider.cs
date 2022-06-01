using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Breakdance
{

    /// <summary>
    /// 
    /// </summary>
    public class TestableAuthenticationStateProvider : AuthenticationStateProvider
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
        }

    }

}
