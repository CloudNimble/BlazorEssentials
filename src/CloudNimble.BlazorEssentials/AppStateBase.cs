using CloudNimble.BlazorEssentials.Extensions;
using CloudNimble.BlazorEssentials.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
        /// The <see cref="AuthenticationStateProvider"/> for the app.
        /// </summary>
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
            }
        }

        /// <summary>
        /// 
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
        /// The instance of the <see cref="IHttpClientFactory" /> injected by the DI system.
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsClaimsPrincipalAuthenticated => ClaimsPrincipal?.Identity?.IsAuthenticated ?? false;

        /// <summary>
        /// The instance of the <see cref="NavigationManager" /> injected by the DI system.
        /// </summary>
        public NavigationManager NavigationManager { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<NavigationItem> NavItems { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigationManager"></param>
        /// <param name="httpClientFactory"></param>
        public AppStateBase(NavigationManager navigationManager, IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
            NavigationManager = navigationManager;
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
        public void Navigate(string uri)
        {
            if (!string.IsNullOrWhiteSpace(uri))
            {
                NavigationManager.NavigateTo(uri);
            }
            SetCurrentNavItem();
        }

        /// <summary>
        /// Tells the AuthenticationProcider to get the latest ClaimsPrincipal and run it through the internal AuthenticationStateChanged handler.
        /// </summary>
        /// <returns></returns>
        public async Task RefreshClaimsPrincipal()
        {
            AuthenticationStateProvider_AuthenticationStateChanged(AuthenticationStateProvider.GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Initilizes <see cref="CurrentNavItem" /> to the proper value based on the current route.
        /// </summary>
        public void SetCurrentNavItem()
        {
            if (CurrentNavItem != null) return;
            SetCurrentNavItem(NavigationManager.ToBaseRelativePath(NavigationManager.Uri));
        }

        /// <summary>
        /// Initilizes <see cref="CurrentNavItem" /> to the proper value based on the current route.
        /// </summary>
        /// <param name="url"></param>
        public void SetCurrentNavItem(string url)
        {
            if (NavItems == null) return;

            var items = NavItems.Traverse(c => c.Children);
            var urlToSet = ToRelativeUrl(url);

            CurrentNavItem = items.Where(c => c.Url != null).FirstOrDefault(c => (urlToSet == string.Empty && ToRelativeUrl(c.Url) == string.Empty) || (ToRelativeUrl(c.Url) != string.Empty && urlToSet.StartsWith(ToRelativeUrl(c.Url), StringComparison.InvariantCultureIgnoreCase)));
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
            //if (state.User?.Identity?.IsAuthenticated == true)
            //{
                ClaimsPrincipal = state.User;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal string ToRelativeUrl(string url)
        { 
            if (url == null) return string.Empty;
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