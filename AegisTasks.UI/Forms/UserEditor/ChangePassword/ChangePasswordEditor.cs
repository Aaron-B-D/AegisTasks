using AegisTasks.UI.Presenters;
using AegisTasks.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AegisTasks.Core.DTO;

namespace AegisTasks.UI.Forms
{
    public partial class ChangePasswordEditor : Form
    {
        public ChangePasswordEditor(UserDTO user)
        {
            InitializeComponent();

            new ChangePasswordPresenter(this, user);
        }

        private void ChangePasswordEditor_Load(object sender, EventArgs e)
        {

        }
    }
}
