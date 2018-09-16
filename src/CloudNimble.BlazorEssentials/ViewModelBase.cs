using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Blazor.Services;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// A base class for your Blazor MVVM implementation that gives you access to all the useful stuff Blazor and BlazorEssentials inject into the app.
    /// </summary>
    /// <typeparam name="TAppState"></typeparam>
    /// <typeparam name="TConfig"></typeparam>
    public class ViewModelBase<TConfig, TAppState>
        where TConfig : ConfigurationBase
        where TAppState : AppStateBase
    {

        #region Properties

        /// <summary>
        /// A boolean signifying whether or not Anonymous users can access the current Page. Defaults to true.
        /// </summary>
        public bool AllowAnonymous { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public List<string> AllowedRoles { get; set; }

        /// <summary>
        /// The injected <see cref="AppStateBase"/> instance for the ViewModel.
        /// </summary>
        public TAppState AppState { get; internal set; }

        /// <summary>
        /// The injected <see cref="ConfigurationBase"/> instance for the ViewModel. 
        /// </summary>
        public TConfig Configuration { get; internal set; }

        /// <summary>
        /// Allows you to set any additional filtering criteria for this ViewModels' HTTP requests from inside the Page itself.
        /// </summary>
        public string FilterCriteria { get; set; }

        /// <summary>
        /// The injected <see cref="HttpClient"/> instance for the ViewModel.
        /// </summary>
        public HttpClient HttpClient { get; internal set; }

        /// <summary>
        /// A boolean signifying whether or not the <see cref="AppState.CurrentUser"/> is authorized to see the Page, given the <see cref="AllowAnonymous"/> and <see cref="AllowedRoles"/> criteria.
        /// </summary>
        public bool IsAuthorized
        {
            get => AllowAnonymous ? true : AppState.IsSignedIn && AllowedRoles.Select(c => AppState.CurrentUser.IsInRole(c)).Any(c => c == true);
        }

        /// <summary>
        /// The injected <see cref="IUriHelper"/> instance for the ViewModel.
        /// </summary>
        public IUriHelper UriHelper { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ViewModelBase"/>.
        /// </summary>
        /// <param name="configuration">The <typeparamref name="TConfig"/> instance injected from the DI container.</param>
        /// <param name="appState">The <typeparamref name="TAppState"/> instance injected from the DI container.</param>
        /// <param name="uriHelper">The <see cref="IUriHelper"/> instance injected from the DI container.</param>
        /// <param name="httpClient">The <see cref="HttpClient"/> instance injected from the DI container.</param>
        public ViewModelBase(TConfig configuration, TAppState appState, IUriHelper uriHelper, HttpClient httpClient)
        {
            Configuration = configuration;
            AppState = appState;
            UriHelper = uriHelper;
            HttpClient = httpClient;
            AllowedRoles = new List<string>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks the <see cref="AppState.CurrentUser"/> authorization and boots the user to the Unauthorized page if required.
        /// </summary>
        public void Authorize()
        {
            if (!IsAuthorized)
            {
                //RWM: If we're not signed in at all, go to the sign in prompt.
                if (!AppState.IsSignedIn) AppState.SignIn();
                //RWM: Otherwise, you're signed in but not allowed to see it. Redirect.
                UriHelper.NavigateTo(Configuration.UnauthorizedRedirectUrl);
            }
        }

        #endregion

    }

}