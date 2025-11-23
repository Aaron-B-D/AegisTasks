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
    public partial class TemplateDetailsViewer : UserControl
    {
        private TemplateDetailsViewerPresenter _Presenter = null;

        // Evento que propaga cuando se guarda un template
        public event EventHandler TemplateSaved;

        public TemplateDetailsViewer()
        {
            InitializeComponent();

            _Presenter = new TemplateDetailsViewerPresenter(this);

            this._Presenter.SavedTemplate += (s, e) => TemplateSaved?.Invoke(this, e);
        }

        public void SetTemplate(TemplateDTO template)
        {
            this._Presenter.SetTemplate(template);
        }

        private void ButtonsContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ParametersGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void GeneralInfoGroupBox_Enter(object sender, EventArgs e)
        {

        }
    }
}
