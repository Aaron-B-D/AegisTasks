using AegisTasks.Core.Common;
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
    public partial class TaskPlanDetailsViewer : UserControl
    {
        public readonly TaskPlanDetailsViewerPresenter Presenter = null;

        // Evento que propaga cuando se guarda un template
        public event EventHandler TemplateSaved;

        public TaskPlanDetailsViewer()
        {
            InitializeComponent();

            this.Presenter = new TaskPlanDetailsViewerPresenter(this);

            this.Presenter.SavedTemplate += (s, e) => TemplateSaved?.Invoke(this, e);
        }

        public void SetTaskPlan(ITaskPlanRegistrable taskPlan)
        {
            this.Presenter.SetTaskPlan(taskPlan);
        }

        private void TaskPlanParametersGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void DescriptionLabel_Click(object sender, EventArgs e)
        {

        }

        private void TaskPlanParametersGroupBox_Enter_1(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void DescriptionTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void DescriptionLabel_Click_1(object sender, EventArgs e)
        {

        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void NameLabel_Click(object sender, EventArgs e)
        {

        }

        private void TaskPlanGeneralInfoGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void ButtonsContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GeneralInfoGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void ParametersGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void SaveAsTemplateButton_Click(object sender, EventArgs e)
        {

        }
    }
}
