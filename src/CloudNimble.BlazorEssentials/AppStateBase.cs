using CloudNimble.BlazorEssentials.Extensions;
using CloudNimble.BlazorEssentials.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// A base class to control application-wide state in a Blazor app.
    /// </summary>
    public class AppStateBase : BlazorObservable
    {

        #region Private Members

        private AuthenticationStateProvider authenticationStateProvider;
        private ClaimsPrincipal claimsPrincipal;
        private NavigationItem currentNavItem;
        private bool disposedValue;

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="AuthenticationStateProvider"/> instance for the application. 
        /// </summary>
        /// <remarks>
        /// This property correctly registers for and de-registers from <see cref="AuthenticationStateProvider"/> events as the 
        /// value is set, and automatically calls <see cref="RefreshClaimsPrincipal"/> for you.
        /// </remarks>
        public AuthenticationStateProvider AuthenticationStateProvider
        {
            get => authenticationStateProvider;
            set
            {
                if (authenticationStateProvider is not null && authenticationStateProvider != value)
                {
                    authenticationStateProvider.AuthenticationStateChanged -= AuthenticationStateProvider_AuthenticationStateChanged;
                }

                Set(() => AuthenticationStateProvider, ref authenticationStateProvider, value);

                if (AuthenticationStateProvider is null) return;
                AuthenticationStateProvider.AuthenticationStateChanged += AuthenticationStateProvider_AuthenticationStateChanged;
                _ = RefreshClaimsPrincipal();
            }
        }

        /// <summary>
        /// The <see cref="ClaimsPrincipal"/> returned from calling <see cref="AuthenticationState.User"/>.
        /// </summary>
        public ClaimsPrincipal ClaimsPrincipal
        {
            get => claimsPrincipal;
            set => Set(() => ClaimsPrincipal, ref claimsPrincipal, value);
        }

        /// <summary>
        /// The <see cref="NavigationItem" /> from <see cref="NavItems" /> that corresponds to the current Route.
        /// </summary>
        public NavigationItem CurrentNavItem
        {
            get => currentNavItem;
            set => Set(() => CurrentNavItem, ref currentNavItem, value);
        }

        /// <summary>
        /// The <see cref="WebAssemblyHostEnvironment"/> injected from the DI container.
        /// </summary>
        public IWebAssemblyHostEnvironment Environment { get; set; }

        /// <summary>
        /// The instance of the <see cref="IHttpClientFactory" /> injected by the DI system.
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get; private set; }

        /// <summary>
        /// Returns a value indicating whether or not the current <see cref="ClaimsPrincipal">ClaimsPrincipal's</see> Identity is authenticated.
        /// </summary>
        public bool IsClaimsPrincipalAuthenticated => ClaimsPrincipal?.Identity?.IsAuthenticated ?? false;

        /// <summary>
        /// 
        /// </summary>
        public IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Allows the application to interact with the browser's History API.
        /// </summary>
        /// <remarks>
        /// This really should be a part of the NavigationManager, but what do we know? ¯\_(ツ)_/¯
        /// </remarks>
        public NavigationHistory NavigationHistory { get; private set; }

        /// <summary>
        /// The instance of the <see cref="NavigationManager" /> injected by the DI system.
        /// </summary>
        public NavigationManager NavigationManager { get; private set; }

        /// <summary>
        /// An <see cref="ObservableCollection{NavigationItem}"/> containing the primary navigation details for the application.
        /// </summary>
        public ObservableCollection<NavigationItem> NavItems { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigationManager">The Blazor <see cref="NavigationManager" /> instance from the DI container.</param>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/> instance from the DI container.</param>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance from the DI container.</param>
        /// <param name="environment">The <see cref="IWebAssemblyHostEnvironment"/> instance from the DI container.</param>
        /// <param name="navHistory">The <see cref="NavigationHistory"/> instance from the DI container.</param>
        /// <param name="stateHasChangedConfig">The <see cref="StateHasChangedConfig"/> instance from the DI container.</param>
        public AppStateBase(NavigationManager navigationManager, IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime, 
            IWebAssemblyHostEnvironment environment, NavigationHistory navHistory, StateHasChangedConfig stateHasChangedConfig = null)
            : base(stateHasChangedConfig)
        {
            NavigationManager = navigationManager;
            HttpClientFactory = httpClientFactory;
            JSRuntime = jsRuntime;
            Environment = environment;
            NavigationHistory = navHistory;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load the NavigationItems into <see cref="NavItems" /> and properly wire up the PropertyChanged event.
        /// </summary>
        /// <param name="items"></param>
        public void LoadNavItems(List<NavigationItem> items)
        {
            NavItems = new ObservableCollection<NavigationItem>(items);
            NavItems.CollectionChanged += (sender, e) => { RaisePropertyChanged(nameof(NavItems)); };
        }

        /// <summary>
        /// Navigates to the specified Uri and sets <see cref="CurrentNavItem" /> to the matching <see cref="NavigationItem" /> in <see cref="NavItems" />.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="setCurrentNavItem">
        /// Determines whether or not we should also set the CurrentNavItem. Usually this is no because the MainLayout should call 
        /// AppState.SetCurrentNavItem in OnParametersSet. This parameter gives you flexibility without potentially calling it twice.
        /// </param>
        public void Navigate(string uri, bool setCurrentNavItem = false)
        {
            // RWM: Navigating to null / whitespace should take you home? 
            if (!string.IsNullOrWhiteSpace(uri))
            {
                NavigationManager.NavigateTo(uri);
            }

            if (setCurrentNavItem)
            {
                SetCurrentNavItem();
            }
        }

        /// <summary>
        /// Utilizes the injected <see cref="NavigationHistory"/> History API to navigate to the last entry in the history stack, and attempts
        /// to set the <see cref="CurrentNavItem"/>.
        /// </summary>
        /// <param name="setCurrentNavItem">
        /// Determines whether or not we should also set the CurrentNavItem. Usually this is no because the MainLayout should call 
        /// AppState.SetCurrentNavItem in OnParametersSet. This parameter gives you flexibility without potentially calling it twice.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the completion state of the operation.</returns>
        /// <remarks>Will not throw an exception if you are at the bottom of the History stack.</remarks>
        public async Task NavigateBackAsync(bool setCurrentNavItem = false)
        {
            await NavigationHistory.Back().ConfigureAwait(false);
            if (setCurrentNavItem)
            {
                SetCurrentNavItem();
            }
        }

        /// <summary>
        /// Utilizes the injected <see cref="NavigationHistory"/> History API to navigate to the next entry in the history stack, and attempts
        /// to set the <see cref="CurrentNavItem"/>.
        /// </summary>
        /// <param name="setCurrentNavItem">
        /// Determines whether or not we should also set the CurrentNavItem. Usually this is no because the MainLayout should call 
        /// AppState.SetCurrentNavItem in OnParametersSet. This parameter gives you flexibility without potentially calling it twice.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the completion state of the operation.</returns>
        /// <remarks>Will not throw an exception if you are at the top of the History stack.</remarks>
        public async Task NavigateForwardAsync(bool setCurrentNavItem = false)
        {
            await NavigationHistory.Forward().ConfigureAwait(false);
            if (setCurrentNavItem)
            {
                SetCurrentNavItem();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <remarks>https://stackoverflow.com/a/62769092</remarks>
        public async Task OpenInNewTab(string url)
        {
             await JSRuntime.InvokeVoidAsync("open", url, "_blank");
        }

        /// <summary>
        /// Tells the AuthenticationProvider to get the latest ClaimsPrincipal and run it through the internal AuthenticationStateChanged handler.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the completion state of the operation.</returns>
        public async Task RefreshClaimsPrincipal()
        {
            AuthenticationStateProvider_AuthenticationStateChanged(AuthenticationStateProvider.GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Initializes <see cref="CurrentNavItem" /> to the proper value based on the current route.
        /// </summary>
        public void SetCurrentNavItem()
        {
            SetCurrentNavItem(NavigationManager.ToBaseRelativePath(NavigationManager.Uri));
        }

        /// <summary>
        /// Initializes <see cref="CurrentNavItem" /> to the proper value based on the current route.
        /// </summary>
        /// <param name="url"></param>
        public void SetCurrentNavItem(string url)
        {
            if (NavItems is null) return;

            var items = NavItems.Traverse(c => c.Children);
            var urlToSet = ToRelativeUrl(url);

            CurrentNavItem = items.Where(c => c.Url is not null).FirstOrDefault(c => (urlToSet == string.Empty && ToRelativeUrl(c.Url) == string.Empty) || (ToRelativeUrl(c.Url) != string.Empty && urlToSet.StartsWith(ToRelativeUrl(c.Url), StringComparison.InvariantCultureIgnoreCase)));
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        private async void AuthenticationStateProvider_AuthenticationStateChanged(Task<AuthenticationState> task)
        {
            var state = await task.ConfigureAwait(false);
            ClaimsPrincipal = state.User;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal string ToRelativeUrl(string url)
        { 
            if (url is null) return string.Empty;
            var absoluteUri = NavigationManager.ToAbsoluteUri(url);
            var baseRelative = NavigationManager.ToBaseRelativePath(absoluteUri.ToString());
            return baseRelative;
        }

        #endregion

        #region Interface Implementations

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (AuthenticationStateProvider is not null)
                    {
                        AuthenticationStateProvider.AuthenticationStateChanged -= AuthenticationStateProvider_AuthenticationStateChanged;
                    }
                }
                disposedValue = true;
            }
        }

        #endregion

    }

}