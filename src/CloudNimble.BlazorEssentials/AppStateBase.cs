using CloudNimble.BlazorEssentials.Navigation;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private readonly BlazorAuthenticationConfig _config;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public NavigationItem CurrentNavItem { get; set; }


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

        /// <summary>
        /// The instance of the <see cref="NavigationManager" /> injected by the DI system.
        /// </summary>
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<NavigationItem> NavItems { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigationManager"></param>
        /// <param name="authOptions"></param>
        public AppStateBase(NavigationManager navigationManager, BlazorAuthenticationConfig authOptions = null)
        {
            NavigationManager = navigationManager;
            _config = authOptions;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initilizes <see cref="CurrentNavItem" /> to the proper value based on the current route.
        /// </summary>
        public void InitializeNav()
        {
            CurrentNavItem = NavItems.FirstOrDefault(c => c.Url.ToUpper().StartsWith(NavigationManager.ToBaseRelativePath(NavigationManager.Uri).ToUpper()));
        }

        /// <summary>
        /// Triggers the authentication mechanism specified in Startup.cs to parse the result from the LoginRedirect and turn it into a ClaimsPrincipal.
        /// </summary>
        /// <param name="token"></param>
        public void ProcessToken(string token)
        {
            CurrentUser = _config.ProcessToken(this, token);
            NavigationManager.NavigateTo("/");
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
        /// Triggers the authentication mechanism specified in Startup.cs to sign the user into the login provider.
        /// </summary>
        public void SignIn()
        {
            if (_config == null)
            {
                throw new ArgumentNullException("authOptions", "You attempted to use the Authentication option without registering a BlazorAuthenticationConfig instance with the IServiceCollection.");
            }
            NavigationManager.NavigateTo(_config.GenerateRedirectUrl());
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