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
    public partial class CopyDirectoryPlanParametersEditor : UserControl
    {
        public readonly CopyDirectoryPlanParametersEditorPresenter Presenter;
        public CopyDirectoryPlanParametersEditor()
        {
            InitializeComponent();
            Presenter = new CopyDirectoryPlanParametersEditorPresenter(this);
        }

        private void CopyDirectoryPlanDetails_Load(object sender, EventArgs e)
        {

        }
    }
}
