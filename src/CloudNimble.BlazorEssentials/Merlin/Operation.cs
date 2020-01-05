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
        private bool isSubmitted;
        private bool isSubmitting;
        private decimal progressPercent;
        private string progressText;
        private string resultText;
        private bool showPanel;
        private ObservableCollection<OperationStep> steps;
        private bool succeeded;
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
        /// A cmputed string that determines what the icon color should be as the Steps change.
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
        /// A computed boolean specifying whether or not all of the OperationSteps have moved past the "InProgress" status.
        /// </summary>
        public bool IsSubmitted
        {
            get => isSubmitted;
            private set
            {
                if (isSubmitted != value)
                {
                    isSubmitted = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A computed boolean specifying whether or not any of the OperationSteps are in the "InProgress" status.
        /// </summary>
        public bool IsSubmitting
        {
            get => isSubmitting;
            private set
            {
                if (isSubmitting != value)
                {
                    isSubmitting = value;
                    RaisePropertyChanged();
                }
            }
        }

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
        /// A computed boolean specifying whether or not ALL of the OperationSteps are successful.
        /// </summary>
        public bool Succeeded
        {
            get => succeeded;
            private set
            {
                if (succeeded != value)
                {
                    succeeded = value;
                    RaisePropertyChanged();
                    ResultText = value ? DisplayText.Success : DisplayText.Failure;
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
        /// <param name="successText"></param>
        /// <param name="failureText"></param>
        public Operation(string title, IEnumerable<OperationStep> steps, string successText, string failureText)
        {
            DisplayIcon = new OperationStatusDisplay("fa-thumbs-up", "fa-thumbs-down", "fa-hourglass fa-pulse");
            DisplayIconColor = new OperationStatusDisplay("text-success", "text-danger", "text-warning");
            DisplayProgressClass = new OperationStatusDisplay("bg-success", "bg-danger", "bg-warning");
            DisplayText = new OperationStatusDisplay(successText, failureText);
            Steps = new ObservableCollection<OperationStep>();
            Title = title;

            Steps.CollectionChanged += Steps_CollectionChanged;
            if (steps == null) return;
            foreach (var step in steps)
            {
                Steps.Add(step);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Changes all of the Steps back to "NotStarted" so the Operation can be run again.
        /// </summary>
        public void Reset()
        {
            Steps.ToList().ForEach(c => c.Reset());
        }

        /// <summary>
        /// Starts the Operation, looping through each step until it is finished.
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            //RWM: We want this to run out-of-band, don't await it.
            Task.Run(async () =>
            {
                Steps.ToList().ForEach(async c =>
                {
                    await c.Start().ConfigureAwait(false);
                    if (c.Status == OperationStepStatus.Failed) return;
                });
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="errorText"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationStep_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var stepsList = Steps.ToList();
            switch (e.PropertyName)
            {
                case nameof(OperationStep.Status):
                    var currentStep = stepsList.FirstOrDefault(c => c.Status == OperationStepStatus.InProgress);
                    ProgressText = currentStep?.DisplayText ?? "";
                    ProgressPercent = decimal.Divide((stepsList.Count(c => (int)c.Status >= 2) * 2) + stepsList.Count(c => c.Status == OperationStepStatus.InProgress), stepsList.Count() * 2);
                    IsSubmitting = stepsList.Any(c => c.Status == OperationStepStatus.InProgress);
                    IsSubmitted = stepsList.All(c => c.Status == OperationStepStatus.Succeeded || c.Status == OperationStepStatus.Failed);
                    Succeeded = IsSubmitted && stepsList.All(c => c.Status == OperationStepStatus.Succeeded);

                    if (stepsList.Any(c => c.Status == OperationStepStatus.Failed))
                    {
                        ShowFailed();
                    }

                    if (IsSubmitted)
                    {
                        if (Succeeded)
                        {
                            ShowSucceeded();
                        }
                        else
                        {
                            ShowFailed();
                        }
                    }
                    if (IsSubmitting)
                    {
                        ShowInProgress();
                    }
                    break;
            }
        }

        private void ShowFailed()
        {
            CurrentIcon = DisplayIcon.Failure;
            CurrentIconColor = DisplayIconColor.Failure;
            CurrentProgressClass = DisplayProgressClass.Failure;
        }

        private void ShowInProgress()
        {
            CurrentIcon = DisplayIcon.InProgress;
            CurrentIconColor = DisplayIconColor.InProgress;
            CurrentProgressClass = DisplayProgressClass.InProgress;
        }

        private void ShowSucceeded()
        {
            CurrentIcon = DisplayIcon.Success;
            CurrentIconColor = DisplayIconColor.Success;
            CurrentProgressClass = DisplayProgressClass.Success;
        }

        #endregion

    }

}
