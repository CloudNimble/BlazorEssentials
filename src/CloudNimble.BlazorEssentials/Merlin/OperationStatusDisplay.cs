using System;
using System.Collections.Generic;
using System.Text;

namespace CloudNimble.BlazorEssentials.Merlin
{

    /// <summary>
    /// 
    /// </summary>
    public class OperationStatusDisplay
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string Failure { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InProgress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Success { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        public OperationStatusDisplay(string success, string failure)
        {
            Failure = failure;
            Success = success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inProgress"></param>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        public OperationStatusDisplay(string success, string failure, string inProgress) : this(success, failure)
        {
            InProgress = inProgress;
        }

        #endregion

    }

}
