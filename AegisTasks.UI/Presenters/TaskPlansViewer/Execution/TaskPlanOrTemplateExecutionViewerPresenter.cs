using AegisTasks.BLL.Aegis;
using AegisTasks.BLL.Common;
using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.Common;
using AegisTasks.Core.DTO;
using AegisTasks.Core.Events;
using AegisTasks.DataAccess.Common.DTO;
using AegisTasks.TasksLibrary.Source.TaskAction.IsDirectoryEmpty;
using AegisTasks.TasksLibrary.TaskAction;
using AegisTasks.TasksLibrary.TaskPlan;
using AegisTasks.UI.Common;
using AegisTasks.UI.Forms;
using AegisTasks.UI.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegisTasks.UI.Presenters {

    public class TaskPlanOrTemplateExecutionViewerPresenter : AegisFormPresenterBase<TaskPlanOrTemplateExecutionViewer>
    {
        private readonly TaskPlanType _TypeToExecute = TaskPlanType.NONE;
        private readonly object _InputParams = null;

        private int _CurrentStep = 0;

        private static readonly Dictionary<string, Func<SupportedLanguage, TaskActionInfo>> AVAILABLE_TASK_ACTIONS =
            new Dictionary<string, Func<SupportedLanguage, TaskActionInfo>>
            {
                {
                    CreateDirectoryTaskAction.CALL_NAME,
                    (language) => new TaskActionInfo
                    {
                        Name = new CreateDirectoryTaskAction().GetName(language),
                        Description = new CreateDirectoryTaskAction().GetDescription(language)
                    }
                },
                {
                    CreateFileTaskAction.CALL_NAME,
                    (language) => new TaskActionInfo
                    {
                        Name = new CreateFileTaskAction().GetName(language),
                        Description = new CreateFileTaskAction().GetDescription(language)
                    }
                },
                {
                    DirectoryExistTaskAction.CALL_NAME,
                    (language) => new TaskActionInfo
                    {
                        Name = new DirectoryExistTaskAction().GetName(language),
                        Description = new DirectoryExistTaskAction().GetDescription(language)
                    }
                },
                {
                    FileExistTaskAction.CALL_NAME,
                    (language) => new TaskActionInfo
                    {
                        Name = new FileExistTaskAction().GetName(language),
                        Description = new FileExistTaskAction().GetDescription(language)
                    }
                },
                {
                    IsFileReadableTaskAction.CALL_NAME,
                    (language) => new TaskActionInfo
                    {
                        Name = new IsFileReadableTaskAction().GetName(language),
                        Description = new IsFileReadableTaskAction().GetDescription(language)
                    }
                },
                {
                    IsFileWritableTaskAction.CALL_NAME,
                    (language) => new TaskActionInfo
                    {
                        Name = new IsFileWritableTaskAction().GetName(language),
                        Description = new IsFileWritableTaskAction().GetDescription(language)
                    }
                },
                {
                    WriteInFileTaskAction.CALL_NAME,
                    (language) => new TaskActionInfo
                    {
                        Name = new WriteInFileTaskAction().GetName(language),
                        Description = new WriteInFileTaskAction().GetDescription(language)
                    }
                },
                {
                    CopyFileTaskAction.CALL_NAME,
                    (language) => new TaskActionInfo
                    {
                        Name = new CopyFileTaskAction().GetName(language),
                        Description = new CopyFileTaskAction().GetDescription(language)
                    }
                },
                {
                    IsDirectoryEmptyTaskAction.CALL_NAME,
                    (language) => new TaskActionInfo
                    {
                        Name = new IsDirectoryEmptyTaskAction().GetName(language),
                        Description = new IsDirectoryEmptyTaskAction().GetDescription(language)
                    }
                },
                {
                    ScanDirectoryTaskAction.CALL_NAME,
                    (language) => new TaskActionInfo
                    {
                        Name = new ScanDirectoryTaskAction().GetName(language),
                        Description = new ScanDirectoryTaskAction().GetDescription(language)
                    }
                }
            };

        private static readonly Dictionary<string, Func<SupportedLanguage, TaskActionInfo>> AVAILABLE_TASK_PLANS =
            new Dictionary<string, Func<SupportedLanguage, TaskActionInfo>>
            {
                {
                    WriteInFilePlan.CALL_NAME,
                    (language) => new TaskActionInfo
                    {
                        Name = new WriteInFilePlan().GetName(language),
                        Description = new WriteInFilePlan().GetDescription(language)
                    }
                },
                {
                    CopyDirectoryPlan.CALL_NAME,
                    (language) => new TaskActionInfo
                    {
                        Name = new CopyDirectoryPlan().GetName(language),
                        Description = new CopyDirectoryPlan().GetDescription(language)
                    }
                }
            };


        private int? _CurrentExecutionId = null;
        private string _CurrentWorkflowId = String.Empty;

        public TaskPlanOrTemplateExecutionViewerPresenter(TaskPlanOrTemplateExecutionViewer form, WriteInFilePlanInputParams writeInFileParams) : base(form)
        {
            _TypeToExecute = TaskPlanType.WRITE_IN_FILE;
            _InputParams = writeInFileParams;
        }

        public TaskPlanOrTemplateExecutionViewerPresenter(TaskPlanOrTemplateExecutionViewer form, CopyDirectoryPlanInputParams copyDirectoryParams) : base(form)
        {
            _TypeToExecute = TaskPlanType.COPY_DIRECTORY;
            _InputParams = copyDirectoryParams;

        }
        public TaskPlanOrTemplateExecutionViewerPresenter(TaskPlanOrTemplateExecutionViewer form, TemplateDTO template) : base(form)
        {
            _TypeToExecute = template.GetTaskPlanType();
            _InputParams = template.GetInputParameters();
        }

        public override void Initialize()
        {
            TaskPlanOrTemplateExecutionViewer view = this._View;

            view.Text = Texts.ExecutionStatus;

            view.FormClosed += (e, sender) =>
            {
                if(!string.IsNullOrWhiteSpace(_CurrentWorkflowId))
                {
                    if(_CurrentExecutionId.HasValue)
                    {
                        ExecutionHistoryDataAccessBLL.UpdateExecution(_CurrentExecutionId.Value, false);
                    }

                    AegisManagerBLL.StopWorkflow(_CurrentWorkflowId);
                }
            };

            execute();
        }

        private void execute()
        {
            this._View.ExecutionHistoryTextBox.Text = string.Empty;
            this._CurrentStep = 0;

            try
            {
                switch (this._TypeToExecute)
                {
                    case TaskPlanType.NONE:
                        Logger.LogError("Se ha intentado ejecutar sin tener ningún taskPlan cargado");
                        this.showError(Texts.ExecutionFailed);
                        this._View.Close();
                        break;
                    case TaskPlanType.WRITE_IN_FILE:
                        this.executeWriteInFile((WriteInFilePlanInputParams)this._InputParams);
                        break;
                    case TaskPlanType.COPY_DIRECTORY:
                        this.executeCopyDirectory((CopyDirectoryPlanInputParams)this._InputParams);
                        break;
                    default:
                        Logger.LogError($"El tipo {this._TypeToExecute} no está soportado");
                        this.showError(Texts.ExecutionFailed);
                        this._View.Close();

                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                this.showError(Texts.ExecutionFailed);
            }
        }

        private void executeWriteInFile(WriteInFilePlanInputParams inputParams)
        {
            try
            {
                Type taskPlanType = new WriteInFilePlan().GetType();

                AegisManagerBLL.ExecuteWriteInPlan(inputParams, this.taskPlanStartedHandler(), this.taskPlanCompletedHandler(), this.taskPlanTerminatedHandler(), this.taskActionStartedHandler(), this.taskActionEndedHandler());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void executeCopyDirectory(CopyDirectoryPlanInputParams inputParams)
        {
            Type taskPlanType = new CopyDirectoryPlan().GetType();


            AegisManagerBLL.ExecuteCopyDirectoryPlan(inputParams, this.taskPlanStartedHandler(), this.taskPlanCompletedHandler(), this.taskPlanTerminatedHandler(), this.taskActionStartedHandler(), this.taskActionEndedHandler());
        }

        protected override bool isLoadAllowed()
        {
            return this.isLogged();
        }

        private TaskPlanEventHandler taskPlanStartedHandler()
        {
            return (sender, e) =>
            {
                TaskActionInfo info = null;

                // Intentamos recuperar la info del plan según WorkflowDefinitionId
                if (AVAILABLE_TASK_PLANS.TryGetValue(e.WorkflowInstance.WorkflowDefinitionId, out var infoFactory))
                {
                    info = infoFactory(SessionManager.CurrentLanguage);
                }

                string currentTaskPlanName = info != null ? info.Name : Texts.UnknownTaskPlan;

                this._CurrentWorkflowId = e.WorkflowInstanceId;

                _CurrentExecutionId = ExecutionHistoryDataAccessBLL.RegisterExecution(
                    e.WorkflowInstanceId,
                    currentTaskPlanName,
                    SessionManager.CurrentUser.Username
                );

                this._CurrentStep++;
                string message = $"▶️ [{currentTaskPlanName}] {Texts.Started.ToUpperInvariant()}";
                this.addHistoryMessage(this._CurrentStep, message);
            };
        }

        // Maneja cuando un workflow se completa
        private TaskPlanEventHandler taskPlanCompletedHandler()
        {
            return (sender, e) =>
            {
                TaskActionInfo info = null;

                if (AVAILABLE_TASK_PLANS.TryGetValue(e.WorkflowInstance.WorkflowDefinitionId, out var infoFactory))
                {
                    info = infoFactory(SessionManager.CurrentLanguage);
                }

                string currentTaskPlanName = info != null ? info.Name : Texts.UnknownTaskPlan;

                this._CurrentWorkflowId = String.Empty;

                if (_CurrentExecutionId.HasValue)
                {
                    ExecutionHistoryDataAccessBLL.UpdateExecution(_CurrentExecutionId.Value, true);
                }

                this._CurrentStep++;
                string message = $"✅ [{currentTaskPlanName}] {Texts.Completed.ToUpperInvariant()}";
                this.addHistoryMessage(this._CurrentStep, message);
            };
        }

        // Maneja cuando un workflow es terminado
        private TaskPlanEventHandler taskPlanTerminatedHandler()
        {
            return (sender, e) =>
            {
                TaskActionInfo info = null;

                if (AVAILABLE_TASK_PLANS.TryGetValue(e.WorkflowInstance.WorkflowDefinitionId, out var infoFactory))
                {
                    info = infoFactory(SessionManager.CurrentLanguage);
                }

                string currentTaskPlanName = info != null ? info.Name : Texts.UnknownTaskPlan;

                if (_CurrentExecutionId.HasValue)
                {
                    ExecutionHistoryDataAccessBLL.UpdateExecution(_CurrentExecutionId.Value, false);
                }

                this._CurrentStep++;
                string message = $"❌ [{currentTaskPlanName}] {Texts.Terminated.ToUpperInvariant()}";
                this.addHistoryMessage(this._CurrentStep, message);
            };
        }


        // Maneja cuando una acción de task se inicia
        private TaskActionEventHandler taskActionStartedHandler()
        {
            return (sender, e) =>
            {
                this._CurrentStep++;

                string message;

                if (string.IsNullOrWhiteSpace(e.StepName))
                {
                    message = $"⚙️ {Texts.InternalTaskAction} {Texts.Started.ToUpperInvariant()}";
                }
                else
                {
                    string stepName = e.StepName;
                    SupportedLanguage lang = SessionManager.CurrentLanguage;

                    string name = stepName;
                    string description = "";

                    if (AVAILABLE_TASK_ACTIONS.TryGetValue(stepName, out var infoFactory))
                    {
                        var info = infoFactory(lang);
                        name = info.Name;
                        description = info.Description;
                    }

                    message = $"▶️ [{name}] {Texts.Started.ToUpperInvariant()} - {description}";
                }

                this.addHistoryMessage(this._CurrentStep, message, 1);
            };
        }


        // Maneja cuando una acción de task finaliza
        private TaskActionEventHandler taskActionEndedHandler()
        {
            return (sender, e) =>
            {
                this._CurrentStep++;

                string message;

                if (string.IsNullOrWhiteSpace(e.StepName))
                {
                    message = $"⚙️ {Texts.InternalTaskAction} {Texts.Completed.ToUpperInvariant()}";
                }
                else
                {
                    string stepName = e.StepName;
                    SupportedLanguage lang = SessionManager.CurrentLanguage;

                    string name = stepName;
                    string description = "";

                    if (AVAILABLE_TASK_ACTIONS.TryGetValue(stepName, out var infoFactory))
                    {
                        var info = infoFactory(lang);
                        name = info.Name;
                        description = info.Description;
                    }

                    message = $"✔️ [{name}] {Texts.Completed.ToUpperInvariant()} - {description}";
                }

                this.addHistoryMessage(this._CurrentStep, message, 1);
            };
        }


        private void addHistoryMessage(int numStep, string message, int indentLevel = 0)
        {
            string indent = new string('\t', indentLevel); // Crea la indentación
            string finalMessage = $"{numStep}. {indent}{message}{Environment.NewLine}";

            if (this._View.ExecutionHistoryTextBox.InvokeRequired)
            {
                this._View.ExecutionHistoryTextBox.Invoke(new Action(() =>
                {
                    this._View.ExecutionHistoryTextBox.AppendText(finalMessage);
                }));
            }
            else
            {
                this._View.ExecutionHistoryTextBox.AppendText(finalMessage);
            }
        }

    }

    class TaskActionInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
