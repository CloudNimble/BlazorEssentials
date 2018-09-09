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
        private Func<AppStateBase, ClaimsPrincipal> _signInFunc;
        private Action<AppStateBase> _signOutAction;
        private Action<AppStateBase> _refreshTokenAction;

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
            _signInFunc = (appStateBase) => { return null; };
            _signOutAction = (s) => { };
            _refreshTokenAction = (s) => { };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signInFunc"></param>
        /// <param name="signOutAction"></param>
        public AppStateBase(Func<AppStateBase, ClaimsPrincipal> signInFunc, Action<AppStateBase> signOutAction, Action<AppStateBase> refreshTokenAction) : base()
        {
            _signInFunc = signInFunc;
            _signOutAction = signOutAction;
            _refreshTokenAction = refreshTokenAction;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Triggers the authentication mechanism specified in Startup.cs to sign the user in.
        /// </summary>
        public void SignIn()
        {
            CurrentUser = _signInFunc(this);
        }

        /// <summary>
        /// Triggers the authentication mechanism specified in Startup.cs to sign the user out.
        /// </summary>
        public void SignOut()
        {
            _signOutAction(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshToken()
        {
            _refreshTokenAction(this);
        }

        #endregion

    }

}