using AegisTasks.Core.TaskAction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AegisTasks.TasksLibrary.TaskAction
{
    /// <summary>
    /// Parámetros de entrada de la acción ScanDirectoryTaskAction
    /// </summary>
    public class ScanDirectoryTaskActionInputParams : TaskActionInputParamsBase
    {

        #region PUBLIC PROPERTIES

        /// <summary>
        /// Directorio raíz a explorar
        /// </summary>
        public DirectoryInfo DirectoryToScan { get; set; }

        /// <summary>
        /// Profundidad máxima a explorar. Si es null, se explora todo
        /// </summary>
        public int? MaxDepth { get; set; }

        #endregion PUBLIC PROPERTIES



        #region CONSTRUCTOR

        public ScanDirectoryTaskActionInputParams(DirectoryInfo directoryToScan, int numRetries, int retryIntervalMs, int? maxDepth) : base(numRetries, retryIntervalMs)
        {
            this.DirectoryToScan = directoryToScan;
            this.MaxDepth = maxDepth;
        }


        public ScanDirectoryTaskActionInputParams(DirectoryInfo directoryToScan, int maxDepth)
        : base()
        {
            DirectoryToScan = directoryToScan;
            MaxDepth = maxDepth;
        }

        public ScanDirectoryTaskActionInputParams(DirectoryInfo directoryToScan)
        : base()
        {
            DirectoryToScan = directoryToScan;
        }

        #endregion CONSTRUCTOR



        #region PUBLIC METHODS

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion PUBLIC METHODS


    }
}
