using System;
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
        public string? Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NextLabel { get; set; } = "NEXT";

        /// <summary>
        /// A computed string containing the DisplayText for the currently running OperationStep.
        /// </summary>
        public bool IsNextEnabled
        {
            get => isNextEnabled;
            set => Set(() => IsNextEnabled, ref isNextEnabled, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public Func<Wizard, Task<bool>> OnNextAction { get; set; } = (wizard) => { return Task.FromResult(true); };

        /// <summary>
        /// 
        /// </summary>
        public Func<Task<bool>> OnBackAction { get; set; } = () => { return Task.FromResult(true); };

        /// <summary>
        /// A <see cref="WizardPaneStatus" /> specifying the current state of this WizardPane.
        /// </summary>
        public WizardPaneStatus Status
        {
            get => status;
            set => Set(() => Status, ref status, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public string? Title { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="stateHasChangedAction"></param>
        public WizardPane(int id, string title, string description, Action stateHasChangedAction)
        {
            Id = id;
            Title = title;
            Description = description;
            StateHasChangedAction = stateHasChangedAction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="stateHasChangedAction"></param>
        /// <param name="nextLabel"></param>
        public WizardPane(int id, string title, string description, Action stateHasChangedAction, string nextLabel)
            : this(id, title, description, stateHasChangedAction)
        {
            NextLabel = nextLabel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="stateHasChangedAction"></param>
        /// <param name="onNextAction"></param>
        /// <param name="nextLabel"></param>
        public WizardPane(int id, string title, string description, Action stateHasChangedAction, string nextLabel, Func<Wizard, Task<bool>> onNextAction)
            : this(id, title, description, stateHasChangedAction, nextLabel)
        {
            OnNextAction = onNextAction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="stateHasChangedAction"></param>
        /// <param name="onNextAction"></param>
        /// <param name="onBackAction"></param>
        /// <param name="nextLabel"></param>
        public WizardPane(int id, string title, string description, Action stateHasChangedAction, string nextLabel, Func<Wizard, Task<bool>> onNextAction, Func<Task<bool>> onBackAction)
            : this(id, title, description, stateHasChangedAction, nextLabel, onNextAction)
        {
            OnBackAction = onBackAction;
        }

        #endregion

    }

}
