using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.Common;
using AegisTasks.DataAccess.Common.DTO;
using AegisTasks.UI.Forms;
using AegisTasks.UI.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegisTasks.UI.Presenters
{
    public class SaveTemplateEditorPresenter : AegisFormPresenterBase<SaveTemplateEditor>
    {
        private readonly TemplateDTO _CurrentTemplate;

        public SaveTemplateEditorPresenter(SaveTemplateEditor form, TemplateDTO currentTemplate) : base(form)
        {
            _CurrentTemplate = currentTemplate;
        }

        public override void Initialize()
        {
            SaveTemplateEditor control = this._View;

            control.SaveButton.Text = Texts.Save;
            control.NameLabel.Text = Texts.Name;
            control.DescriptionLabel.Text = Texts.Description;
            control.Text = Texts.SaveAsTemplate;

            control.NameTextbox.TextChanged += (s, e) =>
            {
                _CurrentTemplate.Name = control.NameTextbox.Text;
            };

            control.DescriptionTextbox.TextChanged += (s, e) =>
            {
                _CurrentTemplate.Description = control.DescriptionTextbox.Text;
            };

            control.SaveButton.Click += (s, e) =>
            {
                string name = _CurrentTemplate.Name;

                if (string.IsNullOrWhiteSpace(name))
                {
                    this.showWarn(Texts.TemplateNameCannotBeEmpty);
                }
                else
                {
                    try
                    {
                        TemplateDataAccessBLL.InsertTemplate(_CurrentTemplate);
                        this.showInfo(Texts.SavedSuccessfully);

                        // Guardado correcto, cerramos el formulario con OK
                        this._View.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this._View.Close();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                        this.showError(Texts.SaveError);

                        // Error al guardar, opcionalmente cerramos con Cancel
                        this._View.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    }
                }
            };
        }

        protected override bool isLoadAllowed()
        {
            return this.isLogged();
        }
    }
}
