using System.Collections.Generic;
using System.Security.Claims;

namespace CloudNimble.BlazorEssentials.Navigation
{

    /// <summary>
    /// Defines an app navigation structure suitable for binding to navigation menus.
    /// </summary>
    public class NavigationItem
    {

        #region Private Members


        #endregion

        #region Properties

        /// <summary>
        /// A string representing the parent category for this <see cref="NavigationItem" />. Can be left blank, but useful for grouping 
        /// <see cref="NavigationItem">NavigationItems</see> into caregories for display.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="Text"/>.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// A string representing the text that will be displayed in the NavBar.
        /// </summary>
        public string Text { get; set; }

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
        /// 
        /// </summary>
        public HashSet<string> Roles { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NavigationItem()
        {
            Roles = new HashSet<string>();
        }

        /// <summary>
        /// Creates a new instance with the minimum-required items to render a Blazor NavLink.
        /// </summary>
        /// <param name="text">A string representing the text that will be displayed in the NavBar.</param>
        /// <param name="icon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="Text"/>.</param>
        /// <param name="url">A string corresponding to the route for this <see cref="NavigationItem" />.</param>
        public NavigationItem(string text, string icon, string url) : this()
        {
            Text = text;
            Icon = icon;
            Url = url;
            IsVisible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">A string representing the text that will be displayed in the NavBar.</param>
        /// <param name="icon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="Text"/>.</param>
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
        /// <param name="icon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="Text"/>.</param>
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
        /// <param name="icon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="Text"/>.</param>
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
        /// <param name="icon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="Text"/>.</param>
        /// <param name="url">A string corresponding to the route for this <see cref="NavigationItem" />.</param>
        /// <param name="category">
        /// A string representing the parent category for this <see cref="NavigationItem" />. Can be useful for grouping 
        /// <see cref="NavigationItem">NavigationItems</see> into caregories for display.
        /// </param>
        /// <param name="isVisible">Specifies whether or not this <see cref="NavigationItem" /> is visible on the NavBar.</param>
        /// <param name="pageTitle">A string representing the text that can be displayed as a page header.</param>
        /// <param name="pageIcon">A string representing the CSS class(es) for the icon that can be displayed next to the <see cref="PageTitle"/> in a page header.</param>
        public NavigationItem(string text, string icon, string url, string category, bool isVisible, string pageTitle, string pageIcon, string roles) : this(text, icon, url, category, isVisible, pageTitle, pageIcon)
        {
            if (string.IsNullOrWhiteSpace(roles)) return;

            foreach (var role in roles.Split(","))
            {
                Roles.Add(role.Trim());
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        public bool IsVisibleToUser(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null)
            {
                //RWM: We may want to change this to throw an ArgumentNullException, but I'm not sure about that yet.
                return false;
            }

            foreach (var role in Roles)
            {
                if (claimsPrincipal.IsInRole(role)) return true;
            }

            return false;
        }


        #endregion

    }

}
