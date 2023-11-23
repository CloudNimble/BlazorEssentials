namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// Represents the basic parts of any HTML element.
    /// </summary>
    public class InterfaceElement
    {

        #region Properties

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

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public InterfaceElement()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayText"></param>
        /// <param name="iconClass"></param>
        /// <param name="cssClass"></param>
        public InterfaceElement(string displayText, string iconClass, string cssClass)
        {
            DisplayText = displayText;
            IconClass = iconClass;
            CssClass = cssClass;
        }

        #endregion

    }

}
