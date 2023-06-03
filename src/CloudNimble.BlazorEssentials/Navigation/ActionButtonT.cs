using System;
using System.Collections.Generic;

namespace CloudNimble.BlazorEssentials.Navigation
{

    /// <summary>
    /// Defines a button that can be used to trigger an action in a User Interface. Useful for binding aq group of actions programmatically.
    /// </summary>
    public class ActionButton<T> : ActionButtonBase
    {

        #region Properties

        /// <summary>
        /// A lambda expression that will be executed when the button is clicked.
        /// </summary>
        public Action<T> ActionMethod { get; set; } = (args) => { };

        #endregion

        #region Constructors

        public ActionButton() : base()
        {
            ActionMethod = (args) => { };
        }

        /// <summary>
        /// Creates a new <see cref="ActionButton"/> instance specifically for icon-only buttons, usually in the header or footer.
        /// </summary>
        /// <param name="iconClass"></param>
        /// <param name="tooltip"></param>
        /// <param name="actionMethod"></param>
        /// <param name="isDisabledFunc"></param>
        public ActionButton(string iconClass, string tooltip, Action<T> actionMethod = null, Func<bool> isDisabledFunc = null) : base(iconClass, tooltip, isDisabledFunc)
        {
            ActionMethod = actionMethod ?? ((args) => { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="buttonClass"></param>
        /// <param name="iconClass"></param>
        /// <param name="popoverName"></param>
        /// <param name="popoverHeader"></param>
        /// <param name="popoverPlacement"></param>
        /// <param name="isDisabledFunc"></param>
        /// <param name="children"></param>
        public ActionButton(string buttonText, string buttonClass, string iconClass,  string popoverName, string popoverHeader, string popoverPlacement, Func<bool> isDisabledFunc = null, List<ActionButton> children = null)
            : base(buttonText, buttonClass, iconClass, popoverName, popoverHeader, popoverPlacement, isDisabledFunc, children)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="buttonClass"></param>
        /// <param name="iconClass"></param>
        /// <param name="actionMethod"></param>
        /// <param name="isDisabledFunc"></param>
        /// <param name="tooltip"></param>
        /// <param name="tooltipContainer"></param>
        public ActionButton(string buttonText, string buttonClass, string iconClass, Action<T> actionMethod = null, Func<bool> isDisabledFunc = null, string tooltip = null, string tooltipContainer = "body")
            : base(buttonText, buttonClass, iconClass, isDisabledFunc, tooltip, tooltipContainer)
        {
            ActionMethod = actionMethod ?? ((args) => { });
        }

        #endregion

    }

}
