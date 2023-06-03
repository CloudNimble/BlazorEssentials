using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CloudNimble.BlazorEssentials.Navigation
{

    /// <summary>
    /// Defines a button that can be used to trigger an action in a User Interface. Useful for binding aq group of actions programmatically.
    /// </summary>
    public abstract class ActionButtonBase
    {

        #region Properties

        /// <summary>
        /// Test to be used for screen readers when this item is rendered.
        /// </summary>
        public string AccessibilityText { get; set; }

        /// <summary>
        /// The CSS class(es) that will be applied to the button tag.
        /// </summary>
        public string ButtonClass { get; set; }

        /// <summary>
        /// The text that will be displayed inside the button.
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ActionButton> Children { get; set; }

        /// <summary>
        /// The CSS class(es) for the icon that will be displayed inside the button.
        /// </summary>
        public string IconClass { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<bool> IsDisabledFunc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ModalTarget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PopoverHeader { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PopoverName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PopoverPlacement { get; set; }

        /// <summary>
        /// The text that will be displayed to the end user when the cursor is hovered over the button.
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TooltipContainer { get; set; }

        #endregion

        #region Constructors

        public ActionButtonBase()
        {
            Children = new();
            IsDisabledFunc = () => false;
        }

        /// <summary>
        /// Creates a new <see cref="ActionButton"/> instance specifically for icon-only buttons, usually in the header or footer.
        /// </summary>
        /// <param name="iconClass"></param>
        /// <param name="tooltip"></param>
        /// <param name="isDisabledFunc"></param>
        public ActionButtonBase(string iconClass, string tooltip, Func<bool> isDisabledFunc = null) : this()
        {
            IconClass = iconClass;
            Tooltip = tooltip;
            IsDisabledFunc = isDisabledFunc ?? (() => false);
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
        public ActionButtonBase(string buttonText, string buttonClass, string iconClass,  string popoverName, string popoverHeader, string popoverPlacement, Func<bool> isDisabledFunc = null, List<ActionButton> children = null) : this()
        {
            ButtonText = buttonText;
            ButtonClass = buttonClass;
            IconClass = iconClass;
            PopoverName = popoverName;
            PopoverHeader = popoverHeader;
            PopoverPlacement = popoverPlacement;
            IsDisabledFunc = isDisabledFunc ?? (() => false);
            Children = children ?? new();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="buttonClass"></param>
        /// <param name="iconClass"></param>
        /// <param name="isDisabledFunc"></param>
        /// <param name="tooltip"></param>
        /// <param name="tooltipContainer"></param>
        public ActionButtonBase(string buttonText, string buttonClass, string iconClass, Func<bool> isDisabledFunc = null, string tooltip = null, string tooltipContainer = "body") : this()
        {
            ButtonText = buttonText;
            ButtonClass = buttonClass;
            IconClass = iconClass;
            IsDisabledFunc = isDisabledFunc ?? (() => false);
            Tooltip = tooltip;
            TooltipContainer = tooltipContainer;
        }

        #endregion

    }

}
