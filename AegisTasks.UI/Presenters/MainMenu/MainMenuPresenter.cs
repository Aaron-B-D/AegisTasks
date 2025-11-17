using AegisTasks.UI.Forms;
using AegisTasks.UI.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegisTasks.UI.Presenters
{
    public class MainMenuPresenter : AegisFormPresenterBase<MainMenu>
    {
        public MainMenuPresenter(MainMenu view) : base(view)
        {
        }

        public override void Initialize()
        {
            _Form.TaskPlansOptionsStripMenu.Text = Texts.TaskPlans;
            _Form.UsersOptionsStripMenu.Text = Texts.Users;
            _Form.HistoryOptionsStripMenu.Text = Texts.History;
            _Form.LogoutOptionLoginStripMenu.Text = Texts.LogOut;
            _Form.Text = Texts.MainMenu;

            _Form.TaskPlansOptionsStripMenu.Click += onOpenTaskPlansViewerButtonClicked;
            _Form.UsersOptionsStripMenu.Click += onOpenUsersEditorButtonClicked;
            _Form.HistoryOptionsStripMenu.Click += onOpenHistoryViewerButtonClicked;
            _Form.LogoutOptionLoginStripMenu.Click += onLogoutButtonClicked;
            _Form.FormClosed += onMainMenuClosed;


        }

        protected void onOpenTaskPlansViewerButtonClicked(object sender, EventArgs e)
        {
            new TaskPlansViewer().ShowDialog(this._Form);
        }

        protected void onOpenUsersEditorButtonClicked(object sender, EventArgs e)
        {
            new UserEditor().ShowDialog(this._Form);
        }

        protected void onOpenHistoryViewerButtonClicked(object sender, EventArgs e)
        {
            new HistoryViewer().ShowDialog(this._Form);
        }

        protected void onLogoutButtonClicked(object sender, EventArgs e)
        {
            SessionManager.Logout();
            this._Form.FireLogoutEvent();
        }

        protected void onMainMenuClosed(object sender, EventArgs e)
        {
            SessionManager.Logout();
        }

        protected override bool isLoadAllowed()
        {
            return this.isLogged();
        }
    }
}
