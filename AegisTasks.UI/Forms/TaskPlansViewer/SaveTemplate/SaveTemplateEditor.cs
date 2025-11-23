using AegisTasks.DataAccess.Common.DTO;
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
    public partial class SaveTemplateEditor : Form
    {
        public SaveTemplateEditor(TemplateDTO templateToSave)
        {
            InitializeComponent();

            new SaveTemplateEditorPresenter(this, templateToSave);
        }

        private void SaveTemplateEditor_Load(object sender, EventArgs e)
        {

        }
    }
}
