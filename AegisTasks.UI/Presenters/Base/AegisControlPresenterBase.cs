using AegisTasks.Core.Common;
using AegisTasks.UI.Forms;
using AegisTasks.UI.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AegisTasks.UI.Presenters
{
    /// <summary>
    /// Presenter especializado para UserControls
    /// </summary>
    public abstract class AegisControlPresenter<ControlType> : AegisPresenterBase<ControlType> where ControlType : UserControl
    {
        protected ControlType _CastedControl
        {
            get
            {
                return _View;
            }
        }

        public AegisControlPresenter(ControlType control) : base(control)
        {
            control.Load += onLoadHandler;
            control.Disposed += onDisposed;
        }

        private void onLoadHandler(object sender, EventArgs e)
        {
            if (!isLoadAllowed())
            {
                MessageBox.Show(
                    Texts.SessionNotFound,
                    Texts.AccessDenied,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                _View.Dispose();
                return;
            }

            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                MessageBox.Show(
                    Texts.FormInitializeError,
                    Texts.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                _View.Dispose();
            }
        }

        /// <summary>
        /// Método virtual para que los presenters hijos puedan hacer limpieza
        /// </summary>
        protected virtual void onDisposed(object sender, EventArgs e)
        {
            // Los hijos pueden sobreescribir para hacer limpieza
        }
    }
}
