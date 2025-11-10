using System;
using System.Collections.Generic;
using System.Text;

namespace AegisTasks.Core.Common
{
    /// <summary>
    /// Define los parámetros de entrada comunes para una acción de tarea.
    /// </summary>
    public interface ITaskActionInputParams: ICloneable
    {
        /// <summary>
        /// Cantidad de intentos que la tarea realizará en caso de fallo antes de considerarse como fallida.
        /// </summary>
        int NumRetries { get; set; }

        /// <summary>
        /// Tiempo de espera en milisegundos entre cada intento de reintento.
        /// </summary>
        int RetryIntervalMs { get; set; }
    }
}
