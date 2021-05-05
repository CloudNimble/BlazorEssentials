using CloudNimble.BlazorEssentials.Extensions;
using CloudNimble.BlazorEssentials.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// A base class to control application-wide state in a Blazor app.
    /// </summary>
    public class AppStateBase : BlazorObservable
    {

        #region Private Members

        private NavigationItem currentNavItem;

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="AuthenticationStateProvider"/> for the app.
        /// </summary>
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        /// <summary>
        /// The <see cref="NavigationItem" /> from <see cref="NavItems" /> that corresponds to the current Route.
        /// </summary>
        public NavigationItem CurrentNavItem 
        {
            get => currentNavItem;
            set
            {
                if (currentNavItem != value)
                {
                    currentNavItem = value;
                    RaisePropertyChanged(() => CurrentNavItem);
                }
            }
        }

        /// <summary>
        /// The instance of the <see cref="IHttpClientFactory" /> injected by the DI system.
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get; private set; }

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
        /// Initilizes <see cref="CurrentNavItem" /> to the proper value based on the current route.
        /// </summary>
        public void SetCurrentNavItem()
        {
            SetCurrentNavItem(NavigationManager.ToBaseRelativePath(NavigationManager.Uri));
        }

        /// <summary>
        /// Initilizes <see cref="CurrentNavItem" /> to the proper value based on the current route.
        /// </summary>
        public void SetCurrentNavItem(string url)
        {
            if (NavItems == null) return;
            CurrentNavItem = NavItems.Traverse(c => c.Children).FirstOrDefault(c => c.Url != null && c.Url.ToUpper().StartsWith(url.ToUpper()));
        }

        #endregion

    }

}