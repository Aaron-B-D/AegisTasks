using AegisTasks.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AegisTasks.Core.TaskAction
{
    public abstract class TaskActionInputParamsBase : ITaskActionInputParams
    {
        #region PROPERTIES

        #region CONSTANTS

        #region PRIVATE CONSTANTS
        #endregion PRIVATE CONSTANTS
        #region PUBLIC CONSTANTS
        #endregion PUBLIC CONSTANTS

        #endregion CONSTANTS

        #region STATIC PROPERTIES
        #region STATIC PRIVATE PROPERTIES

        private readonly static int DEFAULT_NUM_RETRIES = 0;
        private readonly static int DEFAULT_RETRY_INTERVAL_MS = 1000;

        #endregion STATIC PRIVATE PROPERTIES
        #region STATIC PUBLIC PROPERTIES
        #endregion STATIC PUBLIC PROPERTIES
        #endregion STATIC PROPERTIES

        #region PRIVATE PROPERTIES
        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES 

        public int NumRetries { get; set; }
        public int RetryIntervalMs { get; set; }

        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES



        #region METHODS

        #region CONSTRUCTOR

        protected TaskActionInputParamsBase(int numRetries, int retryIntervalMs) { 
            this.NumRetries = numRetries;
            this.RetryIntervalMs = retryIntervalMs;
        }

        protected TaskActionInputParamsBase(): this(DEFAULT_NUM_RETRIES, DEFAULT_RETRY_INTERVAL_MS) {}

        #endregion CONSTRUCTOR

        #region STATIC METHODS

        #region STATIC PRIVATE METHODS
        #endregion STATIC PRIVATE METHODS
        #region STATIC PUBLIC METHODS
        #endregion STATIC PUBLIC METHODS

        #endregion STATIC METHODS

        #region PUBLIC METHODS

        public abstract object Clone();

        #endregion PUBLIC METHODS

        #region PRIVATE METHODS

        #endregion PRIVATE METHODS

        #endregion METHODS

    }
}
