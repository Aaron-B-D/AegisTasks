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
    public partial class UserEditor : Form
    {
        public UserEditor()
        {
            InitializeComponent();

            new UserEditorPresenter(this);
        }

        private void UserEditor_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
