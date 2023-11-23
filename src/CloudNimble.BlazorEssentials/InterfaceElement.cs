namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// Represents the basic parts of any HTML element.
    /// </summary>
    public class InterfaceElement
    {

        /// <summary>
        /// A string representing the CSS classes that will be applied to the element.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// A string representing the text that will be displayed inside the element.
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// A string representing the CSS class for the icon that will be rendered immediately before the <see cref="DisplayText" />.
        /// </summary>
        public string IconClass { get; set; }

    }

}
