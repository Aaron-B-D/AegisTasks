using AegisTasks.BLL.Common;
using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.Common;
using AegisTasks.DataAccess.Common.DTO;
using AegisTasks.TasksLibrary.TaskPlan;
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
    public class TemplateDetailsViewerPresenter : AegisControlPresenter<TemplateDetailsViewer>
    {
        public event EventHandler SavedTemplate;

        private CopyDirectoryPlanParametersEditor _CopyDirectoryPlanParametersEditor = null;
        private WriteInFilePlanParametersEditor _WriteInFilePlamParametersEditor = null;
        private TaskPlanType _Mode = TaskPlanType.NONE;

        private TemplateDTO _Template = null;

        public TemplateDetailsViewerPresenter(TemplateDetailsViewer control) : base(control)
        {

        }

        public override void Initialize()
        {
            TemplateDetailsViewer control = this._CastedControl;

            control.GeneralInfoGroupBox.Text = Texts.GeneralInfo;
            control.ParametersGroupBox.Text = Texts.Parameters;

            control.NameLabel.Text = Texts.Name;
            control.DescriptionLabel.Text = Texts.Description;

            control.ExecuteButton.Text = Texts.Execute;
            control.UpdateTemplateButton.Text = Texts.UpdateTemplate;

            control.NameTextbox.TextChanged += (e, s) =>
            {
                this._Template.Name = control.NameTextbox.Text;
            };

            control.DescriptionTextbox.TextChanged += (e, s) =>
            {
                this._Template.Description = control.DescriptionTextbox.Text;
            };

            control.UpdateTemplateButton.Click += onUpdateTemplateClicked;
            control.ExecuteButton.Click += onExecuteClicked;
        }

        public void SetTemplate(TemplateDTO template)
        {
            TemplateDetailsViewer control = this._CastedControl;

            if (template is null)
            {
                _Template = null;

                control.NameTextbox.Text = String.Empty;
                control.DescriptionTextbox.Text = String.Empty;

                control.UpdateTemplateButton.Enabled = false;
                control.ExecuteButton.Enabled = false;

                this.clearParametersControl();
            } else
            {
                _Template = (TemplateDTO)template.Clone();

                switch (_Template.GetTaskPlanType())
                {
                    case TaskPlanType.WRITE_IN_FILE:
                        {
                            control.NameTextbox.Text = _Template.Name;
                            control.DescriptionTextbox.Text = _Template.Description;

                            this._CopyDirectoryPlanParametersEditor = null;

                            this._WriteInFilePlamParametersEditor = new WriteInFilePlanParametersEditor();
                            loadParametersControl(this._WriteInFilePlamParametersEditor);

                            this._WriteInFilePlamParametersEditor.Presenter.SetParams((WriteInFilePlanInputParams)_Template.GetInputParameters());

                            _Mode = TaskPlanType.WRITE_IN_FILE;

                            control.UpdateTemplateButton.Enabled = true;
                            control.ExecuteButton.Enabled = true;
                        }
                        break;
                    case TaskPlanType.COPY_DIRECTORY:
                        {
                            control.NameTextbox.Text = _Template.Name;
                            control.DescriptionTextbox.Text = _Template.Description;

                            this._WriteInFilePlamParametersEditor = null;

                            this._CopyDirectoryPlanParametersEditor = new CopyDirectoryPlanParametersEditor();
                            loadParametersControl(this._CopyDirectoryPlanParametersEditor);

                            this._CopyDirectoryPlanParametersEditor.Presenter.SetParams((CopyDirectoryPlanInputParams)_Template.GetInputParameters());

                            _Mode = TaskPlanType.COPY_DIRECTORY;

                            control.UpdateTemplateButton.Enabled = true;
                            control.ExecuteButton.Enabled = true;
                            break;
                        }
                    default:
                        {
                            this.clearParametersControl();
                            Logger.LogWarn($"El TaskPlan es de un tipo no soportado: {template.Name}");

                            this.showWarn(Texts.LoadingError);

                            control.UpdateTemplateButton.Enabled = false;
                            control.ExecuteButton.Enabled = false;
                            break;
                        }
                }
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

        private void onUpdateTemplateClicked(object sender, EventArgs e)
        {
            try
            {
                if (this._Template is null)
                {
                    Logger.LogWarn("No se ha cargado un template");
                    this.showError(Texts.TemplateNameCannotBeEmpty);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(this._Template.Name))
                    {
                        this.showError(Texts.TemplateNameCannotBeEmpty);
                    }
                    else
                    {
                        switch (this._Mode)
                        {
                            case TaskPlanType.WRITE_IN_FILE:
                                if (this._WriteInFilePlamParametersEditor.Presenter.AreParamsValid())
                                {
                                    TemplateDTO template = this._Template;
                                    template.SetInputParameters(this._WriteInFilePlamParametersEditor.Presenter.GetParams());

                                    TemplateDataAccessBLL.UpdateTemplate(template);

                                    this.showInfo(Texts.SavedSuccessfully);

                                    this.onSavedTemplate();

                                }
                                else
                                {
                                    this.showError(Texts.ParamsNotValid);
                                }
                                break;

                            case TaskPlanType.COPY_DIRECTORY:
                                if (this._CopyDirectoryPlanParametersEditor.Presenter.AreParamsValid())
                                {
                                    TemplateDTO template = this._Template;
                                    template.SetInputParameters(this._CopyDirectoryPlanParametersEditor.Presenter.GetParams());

                                    TemplateDataAccessBLL.UpdateTemplate(template);

                                    this.showInfo(Texts.SavedSuccessfully);

                                    this.onSavedTemplate();
                                }
                                else
                                {
                                    this.showError(Texts.ParamsNotValid);
                                }
                                break;

                            default:
                                Logger.LogWarn($"El modo {this._Mode} no está soportado");
                                this.showError(Texts.SaveError);
                                break;
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                this.showError(Texts.SaveError);
                Logger.LogException(ex);
            }
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
                        if (this._WriteInFilePlamParametersEditor.Presenter.AreParamsValid())
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

        protected virtual void onSavedTemplate()
        {
            SavedTemplate?.Invoke(this, EventArgs.Empty);
        }
    }
}
