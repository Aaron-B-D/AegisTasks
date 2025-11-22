using AegisTasks.Core.Common;
using AegisTasks.TasksLibrary.TaskPlan;
using AegisTasks.UI.Forms;
using AegisTasks.UI.Language;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegisTasks.UI.Presenters
{
    public class WriteInFilePlanParametersEditorPresenter : TaskPlanParametersPresenterBase<WriteInFilePlanParametersEditor, WriteInFilePlanInputParams>
    {

        public WriteInFilePlanParametersEditorPresenter(WriteInFilePlanParametersEditor control) : base(control)
        {
        }

        public override void Initialize()
        {
            WriteInFilePlanParametersEditor editor = this._CastedControl;

            editor.FilePathLabel.Text = Texts.FilePath;
            editor.SearchButton.Text = Texts.Search;
            editor.AppendContentCheckBox.Text = Texts.AppendContent;
            editor.ContentLabel.Text = Texts.Content;

            editor.FileToWriteDialog.AddExtension = true;
            editor.FileToWriteDialog.Filter = $"{Texts.TextFile} (*.txt)|*.txt";
            editor.FileToWriteDialog.DefaultExt = "txt";

            editor.SearchButton.Click += (s, e) =>
            {
                if (editor.FileToWriteDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string filepath = editor.FileToWriteDialog.FileName;

                    this._CurrentParams.FilePath = new FileInfo(filepath);
                    editor.FilePathTextbox.Text = this._CurrentParams.FilePath.FullName;
                }
            };

            editor.ContentTextBox.TextChanged += (s, e) =>
            {
                this._CurrentParams.Content = editor.ContentTextBox.Text;
            };

        }

        private void clearForm()
        {
            WriteInFilePlanParametersEditor editor = this._CastedControl;

            editor.FilePathTextbox.Text = String.Empty;
            editor.AppendContentCheckBox.Checked = false;
            editor.ContentTextBox.Text = String.Empty;

        }


        private void loadAtForm(WriteInFilePlanInputParams planParams)
        {
            this.clearForm();

            WriteInFilePlanParametersEditor editor = this._CastedControl;

            if (planParams is null)
            {

                editor.FilePathTextbox.Text = String.Empty;
                editor.AppendContentCheckBox.Checked = false;
                editor.ContentTextBox.Text = String.Empty;
            }
            else
            {
                editor.FilePathTextbox.Text = planParams.FilePath?.FullName ?? String.Empty;
                editor.AppendContentCheckBox.Checked = planParams.AppendContent;
                editor.ContentTextBox.Text = planParams.Content?.ToString() ?? String.Empty;
            }
        }

        protected override bool isLoadAllowed()
        {
            return this.isLogged();
        }

        public override void SetParams(WriteInFilePlanInputParams planParams)
        {
            if (planParams is WriteInFilePlanInputParams)
            {
                this._CurrentParams = planParams;

                this.loadAtForm(this._CurrentParams);
            }
            else
            {
                throw new ArgumentException($"El tipo del taskPlan ingresado no es {nameof(WriteInFilePlanInputParams)}");
            }
        }

        public override WriteInFilePlanInputParams GetParams()
        {
            if(this._CurrentParams is null)
            {
                return null;
            }
            else
            {
                WriteInFilePlanInputParams currentParams = this._CurrentParams;

                this._CurrentParams.CreateFileIfNotExists = false;
                this._CurrentParams.CreateDirectoryIfNotExists = false;

                return currentParams;
            }
        }

        public override bool AreParamsValid()
        {
           if (this._CurrentParams is null)
            {
                return false;
            }
            else
            {
                return this._CurrentParams.IsValid();
            }
        }
    }
}
