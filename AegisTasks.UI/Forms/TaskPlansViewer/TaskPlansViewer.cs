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
    public partial class TaskPlansViewer : Form
    {
        public readonly TaskPlansViewerPresenter Presenter = null;

        public TaskPlansViewer()
        {
            InitializeComponent();

            Presenter = new TaskPlansViewerPresenter(this);
        }

        private void TaskPlansViewer_Load(object sender, EventArgs e)
        {

        }

        private void DetailsPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TemplatesGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void SelectorContainer_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
