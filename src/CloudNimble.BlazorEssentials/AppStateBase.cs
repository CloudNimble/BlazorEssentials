using Microsoft.AspNetCore.Components;
using System;
using System.Security.Claims;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// A base class to control application-wide state in a Blazor app.
    /// </summary>
    public class AppStateBase : BlazorObservable
    {

        #region Private Members

        private ClaimsPrincipal _currentUser;
        private readonly IUriHelper _uriHelper;
        private readonly BlazorAuthenticationConfig _config;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ClaimsPrincipal CurrentUser
        {
            get { return _currentUser; }
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    RaisePropertyChanged(() => CurrentUser);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSignedIn => CurrentUser != null /*&& CurrentUser.FindFirst(ClaimTypes.Expiration).Value*/;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriHelper"></param>
        /// <param name="authOptions"></param>
        public AppStateBase(IUriHelper uriHelper, BlazorAuthenticationConfig authOptions = null)
        {
            _uriHelper = uriHelper;
            _config = authOptions;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Triggers the authentication mechanism specified in Startup.cs to sign the user into the login provider.
        /// </summary>
        public void SignIn()
        {
            if (_config == null)
            {
                throw new ArgumentNullException("authOptions", "You attempted to use the Authentication option without registering a BlazorAuthenticationConfig instance with the IServiceCollection.");
            }
            _uriHelper.NavigateTo(_config.GenerateRedirectUrl());
        }

        /// <summary>
        /// Triggers the authentication mechanism specified in Startup.cs to parse the result from the LoginRedirect and turn it into a ClaimsPrincipal.
        /// </summary>
        /// <param name="token"></param>
        public void ProcessToken(string token)
        {
            CurrentUser = _config.ProcessToken(this, token);
            _uriHelper.NavigateTo("/");
        }

        /// <summary>
        /// Triggers the authentication mechanism specified in Startup.cs to refresh an expired token for the currently logged-in user.
        /// </summary>
        public void RefreshToken()
        {
            if (_config == null)
            {
                throw new ArgumentNullException("authOptions", "You attempted to use the Authentication option without registering a BlazorAuthenticationConfig instance with the IServiceCollection.");
            }

            //TODO: Should the action replace the CurrentUser or should we do it? Leaning toward us.
            _config.RefreshToken(this);
        }

        /// <summary>
        /// Triggers the authentication mechanism specified in Startup.cs to sign the user out of the login provider.
        /// </summary>
        public void SignOut()
        {
            if (_config == null)
            {
                throw new ArgumentNullException("authOptions", "You attempted to use the Authentication option without registering a BlazorAuthenticationConfig instance with the IServiceCollection.");
            }

            CurrentUser = null;
            _config.SignOut(this);
        }

        #endregion

    }

}