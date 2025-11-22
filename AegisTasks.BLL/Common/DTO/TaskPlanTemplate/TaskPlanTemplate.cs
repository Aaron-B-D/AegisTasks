using AegisTasks.Core.Common;
using AegisTasks.TasksLibrary.TaskPlan;
using System;
using System.Collections.Generic;
using System.Text;

namespace AegisTasks.BLL.Common
{
    public class TaskPlanTemplate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ITaskPlanRegistrable TaskPlan { get; set; }
        public object InputParams { get; set; }


        public new TaskPlanType GetType()
        {
            Type planType = TaskPlan.GetType();

            if (planType == typeof(WriteInFilePlan))
            {
                return TaskPlanType.WRITE_IN_FILE;
            }
            else if (planType == typeof(CopyDirectoryPlan))
            {
                return TaskPlanType.COPY_DIRECTORY;
            }
            else
            {
                throw new NotSupportedException($"Tipo de plan no soportado: {planType.Name}");
            }
        }



    }
}
