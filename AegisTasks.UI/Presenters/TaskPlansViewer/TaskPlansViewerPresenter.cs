using AegisTasks.BLL.Aegis;
using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.Common;
using AegisTasks.Core.DTO;
using AegisTasks.Core.TaskPlan;
using AegisTasks.UI.Forms;
using AegisTasks.UI.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AegisTasks.UI.Presenters
{
    public class TaskPlansViewerPresenter : AegisFormPresenterBase<TaskPlansViewer>
    {
        private BindingList<ITaskPlanRegistrable> _TaskPlans = new BindingList<ITaskPlanRegistrable>();
        private BindingSource _TaskPlansSource = new BindingSource();

        private BindingList<ITaskPlanRegistrable> _Templates = new BindingList<ITaskPlanRegistrable>();
        private BindingSource _TemplatesSource = new BindingSource();

        private ITaskPlanRegistrable _SelectedTaskPlan = null;

        private TaskPlanDetailsViewer _TaskPlanDetailsMainContainer = new TaskPlanDetailsViewer();
        private TemplateDetailsViewer _TaskPlanTemplateDetails = new TemplateDetailsViewer();

        public TaskPlansViewerPresenter(TaskPlansViewer view) : base(view)
        {

        }

        public override void Initialize()
        {
            TaskPlansViewer view = this._View;
            view.Text = Texts.TaskPlansEditor;
            view.TaskActionGroupBox.Text = Texts.TaskPlans;
            view.TemplatesGroupBox.Text = Texts.Templates;

            this._View.TaskActionsList.Items.Clear();
            HashSet<ITaskPlanRegistrable> availableTaskPlans = AegisManagerBLL.GetAvailableTaskPlans();
            if (availableTaskPlans == null)
            {
                availableTaskPlans =  new HashSet<ITaskPlanRegistrable>();
            }
            _TaskPlans = new BindingList<ITaskPlanRegistrable>(availableTaskPlans.ToList());
            _View.TaskActionsList.Format += (s, e) =>
            {
                if (e.ListItem is ITaskPlanRegistrable plan)
                {
                    e.Value = plan.GetName(SessionManager.CurrentLanguage);
                }
            };
            _View.TaskActionsList.DataSource = _TaskPlans;
            _View.TaskActionsList.SelectedIndexChanged += onTaskActionSelected;

            _SelectedTaskPlan = _TaskPlans.First() ?? null;

            if (_SelectedTaskPlan != null)
            {
                loadTaskPlanAtForm(_SelectedTaskPlan);
            }
        }

        private void onTaskActionSelected(object sender, EventArgs e)
        {
            ITaskPlanRegistrable selected = _View.TaskActionsList.SelectedItem as ITaskPlanRegistrable;
            if (selected != null)
            {
                _SelectedTaskPlan = selected;
                this.loadTaskPlanAtForm(this._SelectedTaskPlan);
            }
        }

        private void loadTaskPlanAtForm(ITaskPlanRegistrable taskPlan)
        {
            loadUserControl(_TaskPlanDetailsMainContainer);
            this._TaskPlanDetailsMainContainer.SetTaskPlan(taskPlan);
        }


        protected override bool isLoadAllowed()
        {
            return this.isLogged();
        }

        private void loadUserControl(UserControl uc)
        {
            _View.DetailsPanel.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            _View.DetailsPanel.Controls.Add(uc);
        }
    }
}
