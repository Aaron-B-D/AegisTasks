using AegisTasks.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegisTasks.UI.Presenters
{
    public class HistoryViewerPresenter : AegisFormPresenterBase<HistoryViewer>
    {
        public HistoryViewerPresenter(HistoryViewer view) : base(view)
        {
        }

        public override void Initialize()
        {
        }

        protected override bool isLoadAllowed()
        {
            return this.isLogged();
        }
    }
}
