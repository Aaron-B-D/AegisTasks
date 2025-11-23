using AegisTasks.BLL.Common;
using AegisTasks.Core.DTO;
using AegisTasks.DataAccess.Common.DTO;
using AegisTasks.TasksLibrary.TaskPlan;
using AegisTasks.UI.Presenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AegisTasks.UI.Forms
{
    public partial class TaskPlanOrTemplateExecutionViewer : Form
    {
        public readonly TaskPlanOrTemplateExecutionViewerPresenter Presenter;

        public TaskPlanOrTemplateExecutionViewer(WriteInFilePlanInputParams writeParams)
        {
            InitializeComponent();

            Presenter = new TaskPlanOrTemplateExecutionViewerPresenter(this, writeParams);

        }


        public TaskPlanOrTemplateExecutionViewer(CopyDirectoryPlanInputParams copyParams)
        {
            InitializeComponent();

            Presenter = new TaskPlanOrTemplateExecutionViewerPresenter(this, copyParams);

        }


        public TaskPlanOrTemplateExecutionViewer(TemplateDTO template)
        {
            InitializeComponent();

            Presenter = new TaskPlanOrTemplateExecutionViewerPresenter(this, template);

        }

        private void TaskPlanExecution_Load(object sender, EventArgs e)
        {

        }

        private void ExecutionHistoryTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
