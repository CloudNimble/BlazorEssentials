using System;

namespace CloudNimble.BlazorEssentials.Navigation
{

    /// <summary>
    /// Defines a button that can be used to trigger an action in a User Interface. Useful for binding aq group of actions programmatically.
    /// </summary>
    public class ActionButton
    {

        /// <summary>
        /// The CSS class(es) that will be applied to the button tag.
        /// </summary>
        public string ButtonClass { get; set; }

        /// <summary>
        /// The text that will be displayed inside the button.
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        /// The CSS class(es) for the icon that will be displayed inside the button.
        /// </summary>
        public string IconClass { get; set; }

        /// <summary>
        /// A lambda expression that will be executed when the button is clicked.
        /// </summary>
        public Action ActionMethod { get; set; }

        /// <summary>
        /// The text that will be displayed to the end user when the cursor is hovered over the button.
        /// </summary>
        public string Tooltip { get; set; }

    }

}
