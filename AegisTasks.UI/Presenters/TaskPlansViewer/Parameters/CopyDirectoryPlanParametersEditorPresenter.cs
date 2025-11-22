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
using System.Windows.Forms;

namespace AegisTasks.UI.Presenters
{
    public class CopyDirectoryPlanParametersEditorPresenter : TaskPlanParametersPresenterBase<CopyDirectoryPlanParametersEditor, CopyDirectoryPlanInputParams>
    {
        public CopyDirectoryPlanParametersEditorPresenter(CopyDirectoryPlanParametersEditor control) : base(control)
        {
        }

        public override void Initialize()
        {
            CopyDirectoryPlanParametersEditor control = this._CastedControl;

            control.CopyDepthLabel.Text = Texts.CopyDepth;
            control.OriginDirectoryLabel.Text = Texts.OriginDirectory;
            control.DestinationDirectoryLabel.Text = Texts.DestinationDirectory;
            control.SearchDestinationDirectoryButton.Text = Texts.Search;
            control.SearchOriginDirectoryButton.Text = Texts.Search;
            control.OverwriteFilesIfExistCheckbox.Text = Texts.OverwriteFilesIfExist;
            control.OverwriteDirectoriesIfExistCheckbox.Text = Texts.OverwriteDirectoriesIfExist;
            control.LimitDepthCheckbox.Text = Texts.LimitDepth;
            control.DepthConfigurationGroupBox.Text = Texts.DepthConfiguration;

            this.clearForm();

            control.LimitDepthCheckbox.CheckedChanged += (sender, args) =>
            {
                if (control.LimitDepthCheckbox.Checked)
                {
                    control.CopyDepthEditor.Enabled = true;
                    this._CurrentParams.CopyDepth = (int)control.CopyDepthEditor.Value;
                }
                else
                {
                    control.CopyDepthEditor.Enabled = false;
                    control.CopyDepthEditor.Value = 0;
                    this._CurrentParams.CopyDepth = null;
                }
            };

            control.CopyDepthEditor.ValueChanged += (sender, args) =>
            {
                if (control.LimitDepthCheckbox.Checked)
                {
                    this._CurrentParams.CopyDepth = (int)control.CopyDepthEditor.Value;
                }
            };

            control.OverwriteDirectoriesIfExistCheckbox.CheckedChanged += (sender, args) =>
            {
                this._CurrentParams.OverwriteDirectoriesIfExist = control.OverwriteDirectoriesIfExistCheckbox.Checked;
            };

            control.OverwriteFilesIfExistCheckbox.CheckedChanged += (sender, args) =>
            {
                this._CurrentParams.OverwriteFilesIfExist = control.OverwriteFilesIfExistCheckbox.Checked;
            };

            control.SearchOriginDirectoryButton.Click += (s, e) =>
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string originPath = folderDialog.SelectedPath;

                        this._CurrentParams.DirectoryToCopy = new DirectoryInfo(originPath);
                        control.OriginDirectoryTextbox.Text = this._CurrentParams.DirectoryToCopy.FullName;
                    }
                }
            };

            control.SearchDestinationDirectoryButton.Click += (s, e) =>
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string destinationPath = folderDialog.SelectedPath;

                        this._CurrentParams.DestinationDirectory = new DirectoryInfo(destinationPath);
                        control.DestinationDirectoryTextBox.Text = this._CurrentParams.DestinationDirectory.FullName;
                    }
                }
            };

        }

        private void clearForm()
        {
            CopyDirectoryPlanParametersEditor control = this._CastedControl;

            control.DestinationDirectoryTextBox.Text = String.Empty;
            control.OriginDirectoryTextbox.Text = String.Empty;
            control.OverwriteDirectoriesIfExistCheckbox.Checked = false;
            control.OverwriteFilesIfExistCheckbox.Checked = false;
            control.LimitDepthCheckbox.Checked = false;
            control.CopyDepthEditor.Value = 0;
            control.CopyDepthEditor.Enabled = false;
        }

        private void loadAtForm(CopyDirectoryPlanInputParams planParams)
        {
            if(planParams == null)
            {
                this.clearForm();
            }
            else
            {
                CopyDirectoryPlanParametersEditor control = this._CastedControl;

                if (planParams.DirectoryToCopy != null)
                {
                    control.OriginDirectoryTextbox.Text = planParams.DirectoryToCopy.FullName;
                }
                else
                {
                    control.OriginDirectoryTextbox.Text = string.Empty;
                }

                // Directorio de destino
                if (planParams.DestinationDirectory != null)
                {
                    control.DestinationDirectoryTextBox.Text = planParams.DestinationDirectory.FullName;
                }
                else
                {
                    control.DestinationDirectoryTextBox.Text = string.Empty;
                }

                // Sobrescribir archivos si existen
                control.OverwriteFilesIfExistCheckbox.Checked = planParams.OverwriteFilesIfExist;

                // Sobrescribir directorios si existen
                control.OverwriteDirectoriesIfExistCheckbox.Checked = planParams.OverwriteDirectoriesIfExist;

                // Límite de profundidad
                if (planParams.CopyDepth.HasValue)
                {
                    control.LimitDepthCheckbox.Checked = true;
                    control.CopyDepthEditor.Enabled = true;
                    control.CopyDepthEditor.Value = planParams.CopyDepth.Value;
                }
                else
                {
                    control.LimitDepthCheckbox.Checked = false;
                    control.CopyDepthEditor.Enabled = false;
                    control.CopyDepthEditor.Value = 0;
                }
            }
        }

        protected override bool isLoadAllowed()
        {
            return this.isLogged();
        }

        public override void SetParams(CopyDirectoryPlanInputParams planParams)
        {
            if (planParams is CopyDirectoryPlanInputParams)
            {
                this._CurrentParams = planParams;

                this.loadAtForm(this._CurrentParams);
            }
            else
            {
                throw new ArgumentException($"El tipo del taskPlan ingresado no es {nameof(CopyDirectoryPlanInputParams)}");
            }
        }

        public override CopyDirectoryPlanInputParams GetParams()
        {
            if(this._CurrentParams is null)
            {
                return null;
            }
            else
            {
                CopyDirectoryPlanInputParams planParams = this._CurrentParams;

                planParams.CreateDestinationDirectoryIfNotExists = false;

                return planParams;
            }
        }

        public override bool AreParamsValid()
        {
            if( this._CurrentParams is null)
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
