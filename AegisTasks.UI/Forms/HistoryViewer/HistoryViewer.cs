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
    public partial class HistoryViewer : Form
    {
        public HistoryViewer()
        {
            InitializeComponent();

            new HistoryViewerPresenter(this);
        }

        private void HistoryViewer_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
