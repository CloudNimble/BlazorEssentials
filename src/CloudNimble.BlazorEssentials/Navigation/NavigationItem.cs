using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace CloudNimble.BlazorEssentials.Navigation
{

    /// <summary>
    /// Defines an app navigation structure suitable for binding to navigation menus.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class NavigationItem : InterfaceElement
    {

        #region Private Members


        #endregion

        #region Properties

        /// <summary>
        /// Test to be used for screen readers when this item is rendered.
        /// </summary>
        public string AccessibilityText { get; set; }

        /// <summary>
        /// A string representing the parent category for this <see cref="NavigationItem" />. Can be left blank, but useful for grouping 
        /// <see cref="NavigationItem">NavigationItems</see> into caregories for display.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<NavigationItem> Children { get; set; }

        /// <summary>
        /// Specifies whether or not this <see cref="NavigationItem" /> is visible on the NavBar. Defaults to True.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="PageTitle"/> in a page header.
        /// </summary>
        public string PageIcon { get; set; }

        /// <summary>
        /// A string representing the text that can be displayed as a page header.
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// A string corresponding to the route for this <see cref="NavigationItem" />.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// A <see cref="HashSet{T}"/> of roles that this NavigationItem is visible to.
        /// </summary>
        public HashSet<string> Roles { get; private set; }

        /// <summary>
        /// A <see cref="bool"/> specifying whether or not this <see cref="NavigationItem"/> is visible when a user is not logged in.
        /// </summary>
        public bool AllowAnonymous { get; }

        /// <summary>
        /// Allows you to set parameters specific to the page that you do NOT want to pass through the Routing system.
        /// </summary>
        /// <remarks>
        /// Accessible through <see cref="AppStateBase.CurrentNavItem"/>.
        /// </remarks>
        public dynamic Parameters { get; set; }

        /// <summary>
        /// Returns a string suitable for display in the debugger. Ensures such strings are compiled by the runtime and not interpreted by the currently-executing language.
        /// </summary>
        /// <remarks>http://blogs.msdn.com/b/jaredpar/archive/2011/03/18/debuggerdisplay-attribute-best-practices.aspx</remarks>
        private string DebuggerDisplay
        {
            get { return $"Text: {DisplayText} | Icon: {IconClass} | Url: {Url}"; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NavigationItem()
        {
            Children = new();
            Roles = new();
        }

        /// <summary>
        /// Creates a new instance with the minimum-required items to render a Blazor NavLink.
        /// </summary>
        /// <param name="text">A string representing the text that will be displayed in the NavBar.</param>
        /// <param name="icon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="InterfaceElement.DisplayText"/>.</param>
        /// <param name="url">A string corresponding to the route for this <see cref="NavigationItem" />.</param>
        public NavigationItem(string text, string icon, string url) : this()
        {
            DisplayText = text;
            IconClass = icon;
            Url = url;
            IsVisible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">A string representing the text that will be displayed in the NavBar.</param>
        /// <param name="icon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="InterfaceElement.DisplayText"/>.</param>
        /// <param name="url">A string corresponding to the route for this <see cref="NavigationItem" />.</param>
        /// <param name="category">
        /// A string representing the parent category for this <see cref="NavigationItem" />. Can be useful for grouping 
        /// <see cref="NavigationItem">NavigationItems</see> into caregories for display.
        /// </param>
        public NavigationItem(string text, string icon, string url, string category) : this(text, icon, url)
        {
            Category = category;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">A string representing the text that will be displayed in the NavBar.</param>
        /// <param name="icon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="InterfaceElement.DisplayText"/>.</param>
        /// <param name="url">A string corresponding to the route for this <see cref="NavigationItem" />.</param>
        /// <param name="category">
        /// A string representing the parent category for this <see cref="NavigationItem" />. Can be useful for grouping 
        /// <see cref="NavigationItem">NavigationItems</see> into caregories for display.
        /// </param>
        /// <param name="isVisible">Specifies whether or not this <see cref="NavigationItem" /> is visible on the NavBar.</param>
        public NavigationItem(string text, string icon, string url, string category, bool isVisible) : this(text, icon, url, category)
        {
            IsVisible = isVisible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">A string representing the text that will be displayed in the NavBar.</param>
        /// <param name="icon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="InterfaceElement.DisplayText"/>.</param>
        /// <param name="category">
        /// A string representing the parent category for this <see cref="NavigationItem" />. Can be useful for grouping 
        /// <see cref="NavigationItem">NavigationItems</see> into caregories for display.
        /// </param>
        /// <param name="isVisible">Specifies whether or not this <see cref="NavigationItem" /> is visible on the NavBar.</param>
        /// <param name="children">A <see cref="List{NavigationItem}"/> containing nodes to render underneath this one.</param>
        public NavigationItem(string text, string icon, string category, bool isVisible, List<NavigationItem> children) : this(text, icon, null, category, isVisible)
        {
            Children = children;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">A string representing the text that will be displayed in the NavBar.</param>
        /// <param name="icon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="InterfaceElement.DisplayText"/>.</param>
        /// <param name="url">A string corresponding to the route for this <see cref="NavigationItem" />.</param>
        /// <param name="category">
        /// A string representing the parent category for this <see cref="NavigationItem" />. Can be useful for grouping 
        /// <see cref="NavigationItem">NavigationItems</see> into caregories for display.
        /// </param>
        /// <param name="isVisible">Specifies whether or not this <see cref="NavigationItem" /> is visible on the NavBar.</param>
        /// <param name="pageTitle">A string representing the text that can be displayed as a page header.</param>
        /// <param name="pageIcon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="PageTitle"/> in a page header.</param>
        public NavigationItem(string text, string icon, string url, string category, bool isVisible, string pageTitle, string pageIcon) : this(text, icon, url, category, isVisible)
        {
            PageTitle = pageTitle;
            PageIcon = pageIcon;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">A string representing the text that will be displayed in the NavBar.</param>
        /// <param name="icon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="InterfaceElement.DisplayText"/>.</param>
        /// <param name="url">A string corresponding to the route for this <see cref="NavigationItem" />.</param>
        /// <param name="category">
        /// A string representing the parent category for this <see cref="NavigationItem" />. Can be useful for grouping 
        /// <see cref="NavigationItem">NavigationItems</see> into caregories for display.
        /// </param>
        /// <param name="isVisible">Specifies whether or not this <see cref="NavigationItem" /> is visible on the NavBar.</param>
        /// <param name="pageTitle">A string representing the text that can be displayed as a page header.</param>
        /// <param name="pageIcon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="PageTitle"/> in a page header.</param>
        /// <param name="roles"></param>
        /// <param name="allowAnonymous"></param>
        public NavigationItem(string text, string icon, string url, string category, bool isVisible, string pageTitle,
                              string pageIcon, string roles, bool allowAnonymous = false) : this(text, icon, url, category, isVisible, pageTitle, pageIcon)
        {
            AllowAnonymous = allowAnonymous;

            if (string.IsNullOrWhiteSpace(roles)) return;

            foreach (var role in roles.Split(","))
            {
                Roles.Add(role.Trim());
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a boolean indicating whether or not the NavItem is available to any of the Roles the <paramref name="claimsPrincipal"/> is in.
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        public bool IsVisibleToUser(ClaimsPrincipal claimsPrincipal)
        {
            //RWM: If no user at all.
            if (claimsPrincipal is null)
            {
                return AllowAnonymous;
            }

            //RWM: If user, but no roles. You're not anonymous, so you should see it.
            if (Roles.Count == 0) return true;

            //RWM: We have roles, and I'm here for it.
            foreach (var role in Roles)
            {
                if (claimsPrincipal.IsInRole(role)) return true;
            }

            //RWM: Sorry, sucker. No dice.
            return false;
        }


        #endregion

    }

}
