using Microsoft.AspNetCore.Blazor.Browser.Services;
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
        private Func<string> _generateRedirectUrlFunc;
        private Func<AppStateBase, string, ClaimsPrincipal> _processTokenFunc;
        private Action<AppStateBase> _refreshTokenAction;
        private Action<AppStateBase> _signOutAction;

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

        //public ClaimsPrincipal CurrentUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSignedIn => CurrentUser != null /*&& CurrentUser.FindFirst(ClaimTypes.Expiration).Value*/;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="AppStateBase"/>.
        /// </summary>
        /// <remarks>
        /// This version is fine if you don't use any Auth features. If your users need to sign in, you'll have to use the other constructor.
        /// </remarks>
        public AppStateBase()
        {
            _generateRedirectUrlFunc = () => { return string.Empty; };
            _processTokenFunc = (appStateBase, token) => { return null; };
            _refreshTokenAction = (s) => { };
            _signOutAction = (s) => { };
        }

        /// <summary>
        /// Creates a new instance of <see cref="AppStateBase"/> suitable for use with Authentication.
        /// </summary>
        /// <param name="generateRedirectUrlFunc">
        /// A <see cref="Func{string}"/> that generates the URL to redirect to for authenticating and returning a JWT.
        /// </param>
        /// <param name="processTokenFunc">
        /// A <see cref="Func{AppStateBase, String, ClaimsPrincipal}"/> that takes an instance <see cref="AppStateBase>"/> and the token to process as
        /// input parameters, and returns a populated ClaimsPrincipal to set as the <see cref="CurrentUser"/>.
        /// </param>
        /// <param name="refreshTokenAction">
        /// An <see cref="Action{AppStateBase}"/> that takes an instance of <see cref="AppStateBase>"/> as an input parameter and refreshes an expired 
        /// token for the currently logged-in user.
        /// </param>
        /// <param name="signOutAction">
        /// An< see cref="Action{AppStateBase}"/> that takes an instance of<see cref= "AppStateBase>" /> as an input parameter and signs the current user
        /// out of the login provider.
        /// </param>
        public AppStateBase(Func<string> generateRedirectUrlFunc, Func<AppStateBase, string, ClaimsPrincipal> processTokenFunc, Action<AppStateBase> refreshTokenAction, 
            Action<AppStateBase> signOutAction) : base()
        {
            _generateRedirectUrlFunc = generateRedirectUrlFunc;
            _processTokenFunc = processTokenFunc;
            _refreshTokenAction = refreshTokenAction;
            _signOutAction = signOutAction;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Triggers the authentication mechanism specified in Startup.cs to sign the user into the login provider.
        /// </summary>
        public void SignIn()
        {
            BrowserUriHelper.Instance.NavigateTo(_generateRedirectUrlFunc());
        }

        /// <summary>
        /// Triggers the authentication mechanism specified in Startup.cs to parse the result from the LoginRedirect and turn it into a ClaimsPrincipal.
        /// </summary>
        /// <param name="token"></param>
        public void ProcessToken(string token)
        {
            CurrentUser = _processTokenFunc(this, token);
            BrowserUriHelper.Instance.NavigateTo("/");
        }

        /// <summary>
        /// Triggers the authentication mechanism specified in Startup.cs to refresh an expired token for the currently logged-in user.
        /// </summary>
        public void RefreshToken()
        {
            //TODO: Should the action replace the CurrentUser or should we do it? Leaning toward us.
            _refreshTokenAction(this);
        }

        /// <summary>
        /// Triggers the authentication mechanism specified in Startup.cs to sign the user out of the login provider.
        /// </summary>
        public void SignOut()
        {
            _signOutAction(this);
            CurrentUser = null;
        }

        #endregion

    }

}