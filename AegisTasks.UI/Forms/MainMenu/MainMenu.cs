using AegisTasks.UI.Presenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AegisTasks.UI.Forms
{
    public partial class MainMenu : Form
    {
        public event EventHandler Logout;

        public MainMenu()
        {
            InitializeComponent();

            new MainMenuPresenter(this);
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        public void FireLogoutEvent()
        {
            Logout?.Invoke(this, EventArgs.Empty);
        }

        private void AegisBannerContainer_Click(object sender, EventArgs e)
        {

        }
    }
}
