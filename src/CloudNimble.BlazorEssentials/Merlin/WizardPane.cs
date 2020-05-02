using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace CloudNimble.BlazorEssentials.Merlin
{

    /// <summary>
    /// 
    /// </summary>
    public class WizardPane : BlazorObservable
    {

        #region Private Members

        private bool isNextEnabled;
        private WizardPaneStatus status;
        private Func<Task<bool>> defaultAction = () => { return Task.FromResult(true); };

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NextLabel { get; set; }

        /// <summary>
        /// A computed string containing the DisplayText for the currently running OperationStep.
        /// </summary>
        public bool IsNextEnabled
        {
            get => isNextEnabled;
            private set
            {
                if (isNextEnabled != value)
                {
                    isNextEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Func<Wizard, Task<bool>> OnNextAction { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<Task<bool>> OnBackAction { get; set; }

        /// <summary>
        /// A <see cref="WizardPaneStatus" /> specifying the current state of this WizardPane.
        /// </summary>
        public WizardPaneStatus Status
        {
            get => status;
            internal set
            {
                if (status != value)
                {
                    status = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region Constructors

        public WizardPane(int id, Func<Wizard, Task<bool>> onNextAction, Func<Task<bool>>? onBackAction, string nextLabel = "NEXT")
        {
            Id = id;
            OnNextAction = onNextAction;
            OnBackAction = onBackAction ?? defaultAction;
            NextLabel = nextLabel;
        }

        #endregion

    }

}
