using System;
using System.Security.Claims;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// 
    /// </summary>
    public class BlazorAuthenticationConfig
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Func<string> GenerateRedirectUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<AppStateBase, string, ClaimsPrincipal> ProcessToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<AppStateBase> RefreshToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<AppStateBase> SignOut { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public BlazorAuthenticationConfig()
        {
            GenerateRedirectUrl = () => { return string.Empty; };
            ProcessToken = (appStateBase, token) => { return null; };
            RefreshToken = (s) => { };
            SignOut = (s) => { };
        }

        /// <summary>
        /// Creates a new instance of <see cref="BlazorAuthenticationConfig"/> suitable for use with Authentication.
        /// </summary>
        /// <param name="generateRedirectUrl">
        /// A <see cref="Func{T, TResult}" /> that generates the URL to redirect to for authenticating and returning a JWT.
        /// </param>
        /// <param name="processToken">
        /// A <see cref="Func{AppStateBase, String, ClaimsPrincipal}"/> that takes an instance <see cref="AppStateBase"/> and the token to process as
        /// input parameters, and returns a populated ClaimsPrincipal to set as the <see cref="AppStateBase.CurrentUser"/>.
        /// </param>
        /// <param name="refreshToken">
        /// An <see cref="Action{AppStateBase}"/> that takes an instance of <see cref="AppStateBase"/> as an input parameter and refreshes an expired 
        /// token for the currently logged-in user.
        /// </param>
        /// <param name="signOut">
        /// An <see cref="Action{AppStateBase}"/> that takes an instance of<see cref= "AppStateBase" /> as an input parameter and signs the current user
        /// out of the login provider.
        /// </param>
        public BlazorAuthenticationConfig(Func<string> generateRedirectUrl, Func<AppStateBase, string, ClaimsPrincipal> processToken, Action<AppStateBase> refreshToken, Action<AppStateBase> signOut)
        {
            GenerateRedirectUrl = generateRedirectUrl;
            ProcessToken = processToken;
            RefreshToken = refreshToken;
            SignOut = signOut;
        }

        #endregion

    }

}