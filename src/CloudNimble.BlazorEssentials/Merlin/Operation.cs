using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Merlin
{

    /// <summary>
    /// A class with observable elements to describe the different components of reporting operation progress to an end user.
    /// </summary>
    public class Operation : BlazorObservable
    {

        #region Private Members

        private string currentIcon;
        private string currentIconColor;
        private string currentProgressClass;
        private decimal progressPercent;
        private string progressText;
        private string resultText;
        private bool shouldObserveStatus;
        private bool showPanel;
        private OperationStatus status;
        private ObservableCollection<OperationStep> steps;
        private string title;

        #endregion

        #region Properties

        /// <summary>
        /// A computed string that determines what the icon should be as the Steps change.
        /// </summary>
        public string CurrentIcon
        {
            get => currentIcon;
            private set
            {
                if (currentIcon != value)
                {
                    currentIcon = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A computed string that determines what the icon color should be as the Steps change.
        /// </summary>
        public string CurrentIconColor
        {
            get => currentIconColor;
            private set
            {
                if (currentIconColor != value)
                {
                    currentIconColor = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A computed string containing the DisplayText for the currently running OperationStep.
        /// </summary>
        public string CurrentProgressClass
        {
            get => currentProgressClass;
            set
            {
                if (currentProgressClass != value)
                {
                    currentProgressClass = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The icon to display to the end user through the Operation lifecycle.
        /// </summary>
        /// <remarks>This property is is not observable.</remarks>
        public OperationStatusDisplay DisplayIcon { get; set; }

        /// <summary>
        /// The colors to use for the <see cref="CurrentIcon" /> through the Operation lifecycle.
        /// </summary>
        /// <remarks>This property is is not observable.</remarks>
        public OperationStatusDisplay DisplayIconColor { get; set; }

        /// <summary>
        /// The CSS Class to use for the Alert's background through the Operation lifecycle.
        /// </summary>
        /// <remarks>This property is is not observable.</remarks>
        public OperationStatusDisplay DisplayProgressClass { get; set; }

        /// <summary>
        /// The text to display to the end user through the Operation lifecycle.
        /// </summary>
        /// <remarks>This property is is not observable.</remarks>
        public OperationStatusDisplay DisplayText { get; set; }

        /// <summary>
        /// A computed number containing the percentage of all the OperationSteps in the "Succeeded" status.
        /// </summary>
        public decimal ProgressPercent
        {
            get => progressPercent;
            private set
            {
                if (progressPercent != value)
                {
                    progressPercent = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A computed string containing the DisplayText for the currently running OperationStep.
        /// </summary>
        public string ProgressText
        {
            get => progressText;
            private set
            {
                if (progressText != value)
                {
                    progressText = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A computed string that determines if we display the SuccessText or FailureText based on the collective outcome of all the Steps.
        /// </summary>
        public string ResultText
        {
            get => resultText;
            private set
            {
                if (resultText != value)
                {
                    resultText = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A computed boolean specifying whether or not the &lt;div&gt; showing the status of this Operation should be displayed.
        /// </summary>
        public bool ShowPanel
        {
            get => showPanel;
            private set
            {
                if (showPanel != value)
                {
                    showPanel = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A computed boolean specifying whether or not ALL of the OperationSteps are successful.
        /// </summary>
        public OperationStatus Status
        {
            get => status;
            private set
            {
                if (status != value)
                {
                    status = value;
                    RaisePropertyChanged();
                    if (shouldObserveStatus)
                    {
                        StateHasChangedAction();
                    }
                }
            }
        }

        /// <summary>
        /// A <see cref="ObservableCollection{OperationStep}" /> containing the different steps of the Operation.
        /// </summary>
        public ObservableCollection<OperationStep> Steps
        {
            get => steps;
            private set
            {
                if (steps != value)
                {
                    steps = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The text to display to the end user regarding what is happening in this step.
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                if (title != value)
                {
                    title = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="steps"></param>
        /// <param name="successText">Initializer for the Success value on the DisplayText property.</param>
        /// <param name="failureText">Initializer for the Failure value on the DisplayText property.</param>
        /// <param name="inProgressText">Initializer for the InProgress value on the DisplayText property.</param>
        /// <param name="notStartedText">Initializer for the NotStarted value on the DisplayText property.</param>
        /// <param name="shouldObserveStatus">Initializer for internal flag indicating if the control should trigger the StateHasChanged action in the <see cref="BlazorObservable"/> when the <see cref="OperationStatus"/> changes.</param>
        public Operation(string title, IEnumerable<OperationStep> steps, string successText, string failureText, string inProgressText = "Working...", string notStartedText = "", bool shouldObserveStatus = true)
        {
            DisplayIcon = new OperationStatusDisplay("fa-thumbs-up", "fa-thumbs-down", "fa-hourglass fa-pulse", "");
            DisplayIconColor = new OperationStatusDisplay("text-success", "text-danger", "text-warning", "");
            DisplayProgressClass = new OperationStatusDisplay("bg-success", "bg-danger", "bg-warning", "");
            DisplayText = new OperationStatusDisplay(successText, failureText, inProgressText, notStartedText);
            Steps = new ObservableCollection<OperationStep>();
            Title = title;

            Steps.CollectionChanged += Steps_CollectionChanged;
            if (steps == null) return;
            foreach (var step in steps)
            {
                Steps.Add(step);
            }
            this.shouldObserveStatus = shouldObserveStatus;
            PropertyChanged += Operation_PropertyChanged;

            // @caldwell0414: set the initial state of the operation
            ShowNotStarted();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="steps"></param>
        public void ReplaceSteps(List<OperationStep> steps)
        {
            if (steps is null)
            {
                throw new ArgumentNullException(nameof(steps));
            }

            Steps.Clear();
            foreach (var step in steps)
            {
                Steps.Add(step);
            }
            RaisePropertyChanged(() => Steps);
        }

        /// <summary>
        /// Changes all of the Steps back to "NotStarted" so the Operation can be run again.
        /// </summary>
        public void Reset()
        {
            Steps.ToList().ForEach(c => c.Reset());
        }

        /// <summary>
        /// Starts the Operation, looping through each step until it is finished or until a step fails.
        /// </summary>
        public void Start()
        {
            //RWM: We want this to run out-of-band, don't await it.
            Task.Run(async () =>
            {
                var shouldContinue = true;
                Steps.ToList().ForEach(async c =>
                {
                    if (shouldContinue)
                    {
                        await c.Start().ConfigureAwait(false);
                        if (c.Status == OperationStepStatus.Failed)
                        {
                            shouldContinue = false;
                        };
                    }
                });
            });
        }

        /// <summary>
        /// Modify the status of a specific <see cref="OperationStep"/>.
        /// </summary>
        /// <param name="id"><see cref="OperationStep"/> identifier.</param>
        /// <param name="status">New <see cref="OperationStepStatus"/>.</param>
        /// <param name="errorText">New value for the ErrorText property on the <see cref="OperationStep"/>.</param>
        public void UpdateStep(int id, OperationStepStatus status, string errorText)
        {
            var step = Steps.ToList().FirstOrDefault(c => c.Id == id);
            if (step == null)
            {
                throw new ArgumentException($"The step with Id '{id}' could not be found in the Steps collection.", nameof(id));
            }

            step.Status = status;
            step.ErrorText = errorText;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Attaches and detaches the OperationStep_PropertyChanged handler as the collection is manipulated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Steps_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (OperationStep item in e.NewItems)
                    {
                        item.PropertyChanged += OperationStep_PropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (OperationStep item in e.OldItems)
                    {
                        item.PropertyChanged -= OperationStep_PropertyChanged;
                    }
                    break;
            }
        }

        private void Operation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Status):
                    BlazorStateChanged();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationStep_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(OperationStep.Status):

                    //@robertmclaws: First update the UI that the particular OperationStep has changed. THEN determine if the Operation is in a different state to trigger UI updates.
                    BlazorStateChanged();

                    var currentStep = Steps.FirstOrDefault(c => c.Status == OperationStepStatus.InProgress);
                    ProgressText = currentStep?.DisplayText ?? "";

                    // @robertmclaws: This handles cases where a > 2 step process fails in the middle. Short-circuit if we've failed.
                    //                Start at the end, assume failure, work backwards.
                    if (Steps.Any(c => c.Status == OperationStepStatus.Failed))
                    {
                        ProgressPercent = 1M;
                        ShowFailed();
                        break;
                    }

                    // @robertmclaws: Everything has to have won in order to win the day.
                    if (Steps.All(c => c.Status == OperationStepStatus.Succeeded))
                    {
                        ProgressPercent = 1M;
                        ShowSucceeded();
                        break;
                    }

                    // @robertmclaws: Handles the Reset case.
                    if (Steps.All(c => c.Status == OperationStepStatus.NotStarted))
                    {
                        ProgressPercent = 0M;
                        ShowNotStarted();
                        break;
                    }

                    // @robertmclaws: Assume everything else is an "in motion" state.
                    // @caldwell0414: only need to calculate the progress percentage during the InProgress status; otherwise, it is a known value (set above)
                    ProgressPercent = decimal.Divide
                    (
                        (Steps.Count(c => (int)c.Status >= 90) * 2)                         // how many steps are done, times 2 because these steps also had a previous "in progress" step
                            + Steps.Count(c => c.Status == OperationStepStatus.InProgress), // how many steps are in progress
                        Steps.Count * 2                                                   // each step in the operation has 2 states that we are concerned about (in progress and either succeeded or failed)
                    );

                    ShowInProgress();
                    break;
            }
        }

        private void BlazorStateChanged()
        {
            if (shouldObserveStatus)
            {
                StateHasChangedAction();
            }
        }

        private void ShowFailed()
        {
            CurrentIcon = DisplayIcon.Failure;
            CurrentIconColor = DisplayIconColor.Failure;
            CurrentProgressClass = DisplayProgressClass.Failure;
            ResultText = DisplayText.Failure;
            Status = OperationStatus.Failed;
        }

        private void ShowInProgress()
        {
            CurrentIcon = DisplayIcon.InProgress;
            CurrentIconColor = DisplayIconColor.InProgress;
            CurrentProgressClass = DisplayProgressClass.InProgress;
            ResultText = DisplayText.InProgress;
            Status = OperationStatus.InProgress;
        }

        private void ShowNotStarted()
        {
            CurrentIcon = DisplayIcon.NotStarted;
            CurrentIconColor = DisplayIconColor.NotStarted;
            CurrentProgressClass = DisplayProgressClass.NotStarted;
            ResultText = DisplayText.NotStarted;
            Status = OperationStatus.NotStarted;
        }

        private void ShowSucceeded()
        {
            CurrentIcon = DisplayIcon.Success;
            CurrentIconColor = DisplayIconColor.Success;
            CurrentProgressClass = DisplayProgressClass.Success;
            ResultText = DisplayText.Success;
            Status = OperationStatus.Succeeded;
        }

        #endregion

    }

}
