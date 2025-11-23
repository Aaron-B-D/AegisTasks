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
            _View.TaskPlansOptionsStripMenu.Text = Texts.TaskPlans;
            _View.UsersOptionsStripMenu.Text = Texts.Users;
            _View.HistoryOptionsStripMenu.Text = Texts.History;
            _View.LogoutOptionLoginStripMenu.Text = Texts.LogOut;
            _View.Text = Texts.MainMenu;

            _View.TaskPlansOptionsStripMenu.Click += onOpenTaskPlansViewerButtonClicked;
            _View.UsersOptionsStripMenu.Click += onOpenUsersEditorButtonClicked;
            _View.HistoryOptionsStripMenu.Click += onOpenHistoryViewerButtonClicked;
            _View.LogoutOptionLoginStripMenu.Click += onLogoutButtonClicked;
            _View.FormClosed += onMainMenuClosed;

            _View.AegisBannerContainer.Image = Images.Images.Aegis_Banner;

        }

        protected void onOpenTaskPlansViewerButtonClicked(object sender, EventArgs e)
        {
            new TaskPlansViewer().ShowDialog(this._View);
        }

        protected void onOpenUsersEditorButtonClicked(object sender, EventArgs e)
        {
            new UserEditor().ShowDialog(this._View);
        }

        protected void onOpenHistoryViewerButtonClicked(object sender, EventArgs e)
        {
            new HistoryViewer().ShowDialog(this._View);
        }

        protected void onLogoutButtonClicked(object sender, EventArgs e)
        {
            SessionManager.Logout();
            this._View.FireLogoutEvent();
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
