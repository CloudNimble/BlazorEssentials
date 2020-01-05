using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Merlin
{

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class OperationStep : BlazorObservable
    {

        #region Private Members

        private string displayText;
        private string errorText;
        private int id;
        private string label;
        private OperationStepStatus status;

        #endregion

        #region Properties

        /// <summary>
        /// The text to display to the end user regarding what is happening in this step.
        /// </summary>
        public string DisplayText
        {
            get => displayText;
            set
            {
                if (displayText != value)
                {
                    displayText = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Any additional text you'd like to display to the end user when there is an error.
        /// </summary>
        public string ErrorText
        {
            get => errorText;
            set
            {
                if (errorText != value)
                {
                    errorText = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A zero-based index identifying this step in relation to other steps in the process.
        /// </summary>
        public int Id
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Returns a Bootstrap Label with the details of the a given <see cref="OperationStep.Status"/>.
        /// </summary>
        public string Label
        {
            get => label;
            private set
            {
                if (label != value)
                {
                    label = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// An async function that returns a boolean indicating whether or not the action succeeded.
        /// </summary>
        public Func<Task<bool>> OnAction { get; set; }

        /// <summary>
        /// An OperationStepStatus where you can change the operation's status and the UI is re-rendered automatically.
        /// </summary>
        public OperationStepStatus Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    status = value;
                    var className = "info";
                    switch (value)
                    {
                        case OperationStepStatus.InProgress:
                            className = "warning";
                            break;
                        case OperationStepStatus.Succeeded:
                            className = "success";
                            break;
                        case OperationStepStatus.Failed:
                            className = "danger";
                            break;
                    }
                    Label = $"<span class='label label-{className}'>{status}</span>";
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Returns a string suitable for display in the debugger. Ensures such strings are compiled by the runtime and not interpreted by the currently-executing language.
        /// </summary>
        /// <remarks>http://blogs.msdn.com/b/jaredpar/archive/2011/03/18/debuggerdisplay-attribute-best-practices.aspx</remarks>
        private string DebuggerDisplay
        {
            get { return $"{Id}: {DisplayText} - {Status}"; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="displayText"></param>
        /// <param name="onAction"></param>
        public OperationStep(int id, string displayText, Func<Task<bool>> onAction)
        {
            Id = id;
            DisplayText = displayText;
            Status = OperationStepStatus.NotStarted;
            OnAction = onAction;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The Action that the Operation calls to trigger a Step. It wrapps the call to onAction with status update logic.
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            //await Task.Run(async () =>
            // {
                 Status = OperationStepStatus.InProgress;
                 var result = await OnAction().ConfigureAwait(false);
                 Status = result ? OperationStepStatus.Succeeded : OperationStepStatus.Failed;
             //    return Task.FromResult(result);
             //}).ConfigureAwait(false);
        }

        /// <summary>
        /// Resets the status of the OperationStep in the event it needs to run again.
        /// </summary>
        public void Reset()
        {
            Status = OperationStepStatus.NotStarted;
            ErrorText = string.Empty;
        }

        #endregion

    }

}