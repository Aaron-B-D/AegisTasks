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
    public partial class WriteInFilePlanParametersEditor : UserControl
    {
        public readonly WriteInFilePlanParametersEditorPresenter Presenter;

        public WriteInFilePlanParametersEditor()
        {
            InitializeComponent();
            Presenter = new WriteInFilePlanParametersEditorPresenter(this);
        }

        private void TaskPlanDetailsPanel_Load(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
