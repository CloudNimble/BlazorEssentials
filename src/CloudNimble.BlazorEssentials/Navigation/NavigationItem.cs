namespace CloudNimble.BlazorEssentials.Navigation
{

    /// <summary>
    /// Defines an app navigation structure suitable for binding to navigation menus.
    /// </summary>
    public class NavigationItem
    {

        /// <summary>
        /// A string representing the parent category for this <see cref="NavigationItem" />. Can be left blank, but useful for grouping 
        /// <see cref="NavigationItem">NavigationItems</see> into caregories for display.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The CSS class(es) for the icon that can be displayed next to the <see cref="Text"/>.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// The text that will be displayed in the NavBar.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Specifies whether or not this <see cref="NavigationItem" /> is visible on the NavBar.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// The CSS class(es) for the icon that can be displayed next to the <see cref="PageTitle"/>.
        /// </summary>
        public string PageIcon { get; set; }

        /// <summary>
        /// The text that can be displayed as a page header.
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// A string corresponding to the route for this <see cref="NavigationItem" />.
        /// </summary>
        public string Url { get; set; }

    }

}
