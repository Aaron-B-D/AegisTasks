using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AegisTasks.Core.Common
{
    /// <summary>
    /// Interfaz que permite registrar TaskActions sin especificar tipos genéricos concretos.
    /// Esto facilita el almacenamiento de diferentes TaskActions con distintos tipos genéricos en una misma colección.
    /// </summary>
    public interface ITaskActionRegistrable
    {
        /// <summary>
        /// Registra la TaskAction en el contenedor de servicios de WorkflowCore.
        /// </summary>
        /// <param name="services">El contenedor de servicios donde se registrará la TaskAction</param>
        void RegisterAtServices(IServiceCollection services);
    }

}
