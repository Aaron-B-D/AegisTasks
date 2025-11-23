using AegisTasks.BLL.Aegis;
using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.Common;
using AegisTasks.Core.DTO;
using AegisTasks.Core.TaskPlan;
using AegisTasks.DataAccess.Common.DTO;
using AegisTasks.UI.Forms;
using AegisTasks.UI.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace AegisTasks.UI.Presenters
{
    public class TaskPlansViewerPresenter : AegisFormPresenterBase<TaskPlansViewer>
    {
        private BindingList<ITaskPlanRegistrable> _TaskPlans = new BindingList<ITaskPlanRegistrable>();
        private BindingSource _TaskPlansSource = new BindingSource();

        private BindingList<TemplateDTO> _Templates = new BindingList<TemplateDTO>();
        private BindingSource _TemplatesSource = new BindingSource();

        private ITaskPlanRegistrable _SelectedTaskPlan = null;
        private TemplateDTO _SelectedTemplate = null;

        private TaskPlanDetailsViewer _TaskPlanDetailsMainContainer = new TaskPlanDetailsViewer();
        private TemplateDetailsViewer _TaskPlanTemplateDetails = new TemplateDetailsViewer();

        public TaskPlansViewerPresenter(TaskPlansViewer view) : base(view)
        {
            // Recargar templates al guardar
            _TaskPlanDetailsMainContainer.TemplateSaved += (s, e) => loadTemplates();

            // Recargar templates al guardar
            _TaskPlanTemplateDetails.TemplateSaved += (s, e) => loadTemplates();
        }

        public override void Initialize()
        {
            TaskPlansViewer view = _View;
            view.Text = Texts.TaskPlansEditor;
            view.TaskActionGroupBox.Text = Texts.TaskPlans;
            view.TemplatesGroupBox.Text = Texts.Templates;

            // Inicializar TaskPlans
            HashSet<ITaskPlanRegistrable> availableTaskPlans = AegisManagerBLL.GetAvailableTaskPlans() ?? new HashSet<ITaskPlanRegistrable>();
            _TaskPlans = new BindingList<ITaskPlanRegistrable>(availableTaskPlans.ToList());
            _TaskPlansSource.DataSource = _TaskPlans;
            view.TaskActionsList.DataSource = _TaskPlansSource;
            view.TaskActionsList.Format += (s, e) =>
            {
                if (e.ListItem is ITaskPlanRegistrable plan)
                    e.Value = plan.GetName(SessionManager.CurrentLanguage);
            };
            view.TaskActionsList.SelectedIndexChanged += onTaskActionSelected;

            // Inicializar Templates
            _TemplatesSource.DataSource = _Templates;
            view.TemplatesList.DataSource = _TemplatesSource;
            view.TemplatesList.Format += (s, e) =>
            {
                if (e.ListItem is TemplateDTO template)
                    e.Value = template.Name;
            };
            view.TemplatesList.SelectedIndexChanged += onTemplateSelected;

            // Cargar TaskPlan inicial: solo el primero
            _SelectedTaskPlan = _TaskPlans.FirstOrDefault();
            _SelectedTemplate = null;

            // Forzar que la lista de templates no tenga selección al iniciar
            view.TemplatesList.ClearSelected();

            if (_SelectedTaskPlan != null)
                loadTaskPlanInForm(_SelectedTaskPlan);

            loadTemplates();
        }

        protected override bool isLoadAllowed()
        {
            return isLogged();
        }

        private void onTaskActionSelected(object sender, EventArgs e)
        {
            if (_View.TaskActionsList.SelectedItem is ITaskPlanRegistrable selected)
            {
                // Deseleccionar templates
                _View.TemplatesList.ClearSelected();
                _SelectedTemplate = null;

                _SelectedTaskPlan = selected;
                loadTaskPlanInForm(_SelectedTaskPlan);
            }
        }

        private void onTemplateSelected(object sender, EventArgs e)
        {
            if (_View.TemplatesList.SelectedItem is TemplateDTO selected)
            {
                // Deseleccionar TaskPlans
                _View.TaskActionsList.ClearSelected();
                _SelectedTaskPlan = null;

                _SelectedTemplate = selected;
                loadTemplate(_SelectedTemplate);
            }
        }

        private void loadTaskPlanInForm(ITaskPlanRegistrable taskPlan)
        {
            loadUserControl(_TaskPlanDetailsMainContainer);
            _TaskPlanDetailsMainContainer.SetTaskPlan(taskPlan);
        }

        private void loadTemplate(TemplateDTO template)
        {
            loadUserControl(_TaskPlanTemplateDetails);
            _TaskPlanTemplateDetails.SetTemplate(template);
        }

        private void loadTemplates()
        {
            _Templates.Clear();
            var templates = TemplateDataAccessBLL.GetTemplates(SessionManager.CurrentUser.Username);
            foreach (var t in templates)
                _Templates.Add(t);

            // Forzar que la lista de templates no tenga selección tras recargar
            _View.TemplatesList.ClearSelected();
            _SelectedTemplate = null;
        }

        private void loadUserControl(UserControl uc)
        {
            _View.DetailsPanel.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            _View.DetailsPanel.Controls.Add(uc);
        }
    }
}
