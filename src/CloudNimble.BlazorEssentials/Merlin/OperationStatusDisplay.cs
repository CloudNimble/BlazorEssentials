using System;
using System.Collections.Generic;
using System.Text;

namespace CloudNimble.BlazorEssentials.Merlin
{

    /// <summary>
    /// An object to store display values represented by specific statuses for an <see cref="Operation"/> or <see cref="OperationStep"/>.
    /// </summary>
    public class OperationStatusDisplay
    {

        #region Properties

        /// <summary>
        /// Text to display when the operation has not started.
        /// </summary>
        public string NotStarted { get; set; }

        /// <summary>
        /// Text to display when the operation has failed.
        /// </summary>
        public string Failure { get; set; }

        /// <summary>
        /// Text to display while the operation is in progress.
        /// </summary>
        public string InProgress { get; set; }

        /// <summary>
        /// Text to display when the operation has succeeded.
        /// </summary>
        public string Success { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor overload to set only success / failure texts.
        /// </summary>
        /// <param name="success">Initializer for Success text.</param>
        /// <param name="failure">Initializer for Failure text.</param>
        public OperationStatusDisplay(string success, string failure)
        {
            Failure = failure;
            Success = success;
        }

        /// <summary>
        /// Constructor overload to set all texts.
        /// </summary>
        /// <param name="inProgress">Initializer for InProgress text.</param>
        /// <param name="notStarted">Initializer for NotStarted text.</param>
        /// <param name="success">Initializer for Success text.</param>
        /// <param name="failure">Initializer for Failure text.</param>
        public OperationStatusDisplay(string success, string failure, string inProgress, string notStarted) : this(success, failure)
        {
            InProgress = inProgress;
            NotStarted = notStarted;
        }

        #endregion

    }

}
