using AegisTasks.Core.Common;
using AegisTasks.TasksLibrary.TaskPlan;
using AegisTasks.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AegisTasks.UI.Presenters
{
    public abstract class TaskPlanParametersPresenterBase<ControlType, InputParamsType> : AegisControlPresenter<ControlType> where ControlType : UserControl
    {
        protected InputParamsType _CurrentParams;

        /// <summary>
        /// Carga en el panel el TaskPlan ingresado
        /// </summary>
        /// <param name="planParams"></param>
        public abstract void SetParams(InputParamsType planParams);

        public abstract InputParamsType GetParams();

        public abstract bool AreParamsValid();

        protected TaskPlanParametersPresenterBase(ControlType control) : base(control)
        {
        }
    }
}
