using AegisTasks.BLL.Common;
using AegisTasks.Core.Common;
using AegisTasks.TasksLibrary.TaskPlan;
using AegisTasks.UI.Common;
using AegisTasks.UI.Forms;
using AegisTasks.UI.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AegisTasks.UI.Presenters
{
    public class TaskPlanDetailsViewerPresenter: AegisControlPresenter<TaskPlanDetailsViewer>
    {
        private CopyDirectoryPlanParametersEditor _CopyDirectoryPlanParametersEditor = null;
        private WriteInFilePlanParametersEditor _WriteInFilePlamParametersEditor = null;
        private TaskPlanType _Mode = TaskPlanType.NONE;


        public TaskPlanDetailsViewerPresenter(TaskPlanDetailsViewer control) : base(control)
        {

        }

        public override void Initialize()
        {
            TaskPlanDetailsViewer control = this._CastedControl;

            control.GeneralInfoGroupBox.Text = Texts.GeneralInfo;
            control.ParametersGroupBox.Text = Texts.Parameters;

            control.NameLabel.Text = Texts.Name;
            control.DescriptionLabel.Text = Texts.Description;

            control.ExecuteButton.Text = Texts.Execute;
            control.SaveAsTemplateButton.Text = Texts.SaveAsTemplate;

            control.SaveAsTemplateButton.Click += onSaveAsTemplateClicked;
            control.ExecuteButton.Click += onExecuteClicked;

        }

        public void SetTaskPlan(ITaskPlanRegistrable taskPlanRegistrable)
        {
            TaskPlanDetailsViewer control = this._CastedControl;

            if (taskPlanRegistrable is null)
            {
                control.NameTextbox.Text = String.Empty;
                control.DescriptionTextbox.Text = String.Empty;

                control.SaveAsTemplateButton.Enabled = false;
                control.ExecuteButton.Enabled = false;

                this.clearParametersControl();
            }
            else if(taskPlanRegistrable is CopyDirectoryPlan)
            {
                control.NameTextbox.Text = taskPlanRegistrable.GetName(SessionManager.CurrentLanguage);
                control.DescriptionTextbox.Text = taskPlanRegistrable.GetDescription(SessionManager.CurrentLanguage);

                this._WriteInFilePlamParametersEditor = null;

                this._CopyDirectoryPlanParametersEditor = new CopyDirectoryPlanParametersEditor();
                loadParametersControl(this._CopyDirectoryPlanParametersEditor);

                this._CopyDirectoryPlanParametersEditor.Presenter.SetParams(new CopyDirectoryPlanInputParams(false, false, false, null, null));

                _Mode = TaskPlanType.COPY_DIRECTORY;

                control.SaveAsTemplateButton.Enabled = true;
                control.ExecuteButton.Enabled = true;
            }
            else if(taskPlanRegistrable is WriteInFilePlan)
            {
                control.NameTextbox.Text = taskPlanRegistrable.GetName(SessionManager.CurrentLanguage);
                control.DescriptionTextbox.Text = taskPlanRegistrable.GetDescription(SessionManager.CurrentLanguage);

                this._CopyDirectoryPlanParametersEditor = null;

                this._WriteInFilePlamParametersEditor = new WriteInFilePlanParametersEditor();
                loadParametersControl(this._WriteInFilePlamParametersEditor);

                this._WriteInFilePlamParametersEditor.Presenter.SetParams(new WriteInFilePlanInputParams(null, String.Empty, false, false, false));

                _Mode = TaskPlanType.WRITE_IN_FILE;

                control.SaveAsTemplateButton.Enabled = true;
                control.ExecuteButton.Enabled = true;
            }
            else
            {
                this.clearParametersControl();
                Logger.LogWarn($"El TaskPlan es de un tipo no soportado: {taskPlanRegistrable.GetName(SupportedLanguage.SPANISH)}");

                this.showWarn(Texts.LoadingError);

                control.SaveAsTemplateButton.Enabled = false;
                control.ExecuteButton.Enabled = false;
            }
           
        }

        protected override bool isLoadAllowed()
        {
            return this.isLogged();
        }

        private void clearParametersControl()
        {
            _View.ParametersGroupBox.Controls.Clear();
        }

        private void loadParametersControl(UserControl uc)
        {
            this.clearParametersControl();
            uc.Dock = DockStyle.Fill;
            _View.ParametersGroupBox.Controls.Add(uc);
        }

        private void onSaveAsTemplateClicked(object sender, EventArgs e)
        {

        }

        private void onExecuteClicked(object sender, EventArgs e)
        {
            switch (this._Mode)
            {
                case TaskPlanType.NONE:
                    Logger.LogWarn($"No hay un TaskPlan cargado en el editor y sin embargo se ha podido clicar en el botón de ejecutar");
                    break;
                case TaskPlanType.WRITE_IN_FILE:
                    {
                        if(this._WriteInFilePlamParametersEditor.Presenter.AreParamsValid())
                        {
                            new TaskPlanOrTemplateExecutionViewer(this._WriteInFilePlamParametersEditor.Presenter.GetParams()).ShowDialog(this._View);
                        }
                        else
                        {
                            this.showError(Texts.ParamsNotValid);
                        }
                        break;
                    }
                case TaskPlanType.COPY_DIRECTORY:
                    {
                        if (this._CopyDirectoryPlanParametersEditor.Presenter.AreParamsValid())
                        {
                            new TaskPlanOrTemplateExecutionViewer(this._CopyDirectoryPlanParametersEditor.Presenter.GetParams()).ShowDialog(this._View);
                        }
                        else
                        {
                            this.showError(Texts.ParamsNotValid);
                        }
                        break;
                    }
                default:
                    Logger.LogWarn($"El modo {this._Mode} no está soportado");
                    break;
            }
        }
    }
}