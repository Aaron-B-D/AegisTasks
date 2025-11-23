using System;
using System.Collections.Generic;
using System.Text;

namespace AegisTasks.Core.TaskPlan
{
    public abstract class TaskPlanInputParamsBase<InputParamsClass>
    {
        public abstract string ToJson();

    }
}
