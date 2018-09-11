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
        /// 
        /// </summary>
        public AppStateBase()
        {
            _generateRedirectUrlFunc = () => { return string.Empty; };
            _processTokenFunc = (appStateBase, token) => { return null; };
            _refreshTokenAction = (s) => { };
            _signOutAction = (s) => { };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateRedirectUrlFunc"></param>
        /// <param name="processTokenFunc"></param>
        /// <param name="refreshTokenAction"></param>
        /// <param name="signOutAction"></param>
        public AppStateBase(Func<string> generateRedirectUrlFunc, Func<AppStateBase, string, ClaimsPrincipal> processTokenFunc, Action<AppStateBase> refreshTokenAction, Action<AppStateBase> signOutAction) : base()
        {
            _generateRedirectUrlFunc = generateRedirectUrlFunc;
            _processTokenFunc = processTokenFunc;
            _refreshTokenAction = refreshTokenAction;
            _signOutAction = signOutAction;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Triggers the authentication mechanism specified in Startup.cs to sign the user in.
        /// </summary>
        public void SignIn()
        {
            BrowserUriHelper.Instance.NavigateTo(_generateRedirectUrlFunc());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public void ProcessToken(string token)
        {
            CurrentUser = _processTokenFunc(this, token);
            BrowserUriHelper.Instance.NavigateTo("/");
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshToken()
        {
            //TODO: Should the action replace the CurrentUser or should we do it? Leaning toward us.
            _refreshTokenAction(this);
        }

        /// <summary>
        /// Triggers the authentication mechanism specified in Startup.cs to sign the user out.
        /// </summary>
        public void SignOut()
        {
            _signOutAction(this);
            CurrentUser = null;
        }

        #endregion

    }

}