using System;

namespace CloudNimble.BlazorEssentials.Merlin
{

    /// <summary>
    /// 
    /// </summary>
    public class WizardConfirmationPane : WizardPane
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="stateHasChangedAction"></param>
        /// <param name="nextLabel"></param>
        public WizardConfirmationPane(int id, string title, string description, Action stateHasChangedAction, string nextLabel = "NEXT")
            : base(id, title, description, stateHasChangedAction, nextLabel)
        {
        }

    }

}
