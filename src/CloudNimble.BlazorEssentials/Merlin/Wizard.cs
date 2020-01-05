using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Merlin
{

    /// <summary>
    /// 
    /// </summary>
    public class Wizard : BlazorObservable
    {

        #region Private Members

        private WizardPane currentPane;
        private ObservableCollection<WizardPane> panes;
        private Operation operation;
        private bool isBackVisible;
        private bool isFinishVisible;
        private bool isOperationStartVisible;

        #endregion

        #region Properties

        /// <summary>
        /// A computed string containing the DisplayText for the currently running OperationStep.
        /// </summary>
        public WizardPane CurrentPane
        {
            get => currentPane;
            private set
            {
                if (currentPane != value)
                {
                    currentPane = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A computed string containing the DisplayText for the currently running OperationStep.
        /// </summary>
        public bool IsBackVisible
        {
            get => isBackVisible;
            private set
            {
                if (isBackVisible != value)
                {
                    isBackVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A computed string containing the DisplayText for the currently running OperationStep.
        /// </summary>
        public bool IsFinishVisible
        {
            get => isFinishVisible;
            private set
            {
                if (isFinishVisible != value)
                {
                    isFinishVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A computed string containing the DisplayText for the currently running OperationStep.
        /// </summary>
        public bool IsOperationStartVisible
        {
            get => isOperationStartVisible;
            private set
            {
                if (isOperationStartVisible != value)
                {
                    isOperationStartVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A computed string containing the DisplayText for the currently running OperationStep.
        /// </summary>
        public Operation Operation
        {
            get => operation;
            private set
            {
                if (operation != value)
                {
                    operation = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A computed string containing the DisplayText for the currently running OperationStep.
        /// </summary>
        public ObservableCollection<WizardPane> Panes
        {
            get => panes;
            private set
            {
                if (panes != value)
                {
                    panes = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="panes"></param>
        /// <param name="operation"></param>
        public Wizard(string title, List<WizardPane> panes, Operation operation)
        {
            Title = title;
            Panes = new ObservableCollection<WizardPane>();
            Operation = operation;
            if (panes != null)
            {
                foreach (var pane in panes)
                {
                    Panes.Add(pane);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Back()
        {
            if (CurrentPane == null) return;
            CurrentPane.Status = WizardPaneStatus.NotStarted;
            CurrentPane = Panes[Panes.IndexOf(CurrentPane) - 1];
            CurrentPane.Status = WizardPaneStatus.NotStarted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Next()
        {
            if (IsFinishVisible) return;
            if (CurrentPane == null) return;
            if (CurrentPane.OnNextAction != null)
            {
                CurrentPane.Status = WizardPaneStatus.InProgress;
                var result = await CurrentPane.OnNextAction(this).ConfigureAwait(false);
                CurrentPane.Status = result ? WizardPaneStatus.Succeeded : WizardPaneStatus.Failed;
            }
            else
            {
                CurrentPane.Status = WizardPaneStatus.Succeeded;
            }

            if (CurrentPane is WizardConfirmationPane) return;
            var index = Panes.IndexOf(CurrentPane);
            if (index == Panes.Count - 1) return;
            CurrentPane = Panes[index + 1];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Reset()
        {
            foreach (var pane in Panes)
            {
                pane.Status = WizardPaneStatus.NotStarted;
            }
            CurrentPane = Panes[0];
            if (Operation != null)
            {
                Operation.Reset();
            }
        }

        #endregion

    }

}
