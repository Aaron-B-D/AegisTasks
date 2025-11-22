using AegisTasks.Core.Common;
using AegisTasks.UI.Language;
using AegisTasks.UI.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AegisTasks.UI.Presenters
{
    /// <summary>
    /// Presenter especializado para Forms
    /// </summary>
    public abstract class AegisFormPresenterBase<FormType> : AegisPresenterBase<FormType> where FormType : Form
    {
        public AegisFormPresenterBase(FormType form) : base(form)
        {
            form.Load += onLoadHandler;
            form.FormClosing += onFormClosing;
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
                _View.Close();
                return;
            }

            try
            {
                _View.Icon = Images.Images.AegisLogo;
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
                _View.Close();
            }
        }

        /// <summary>
        /// Método virtual para que los presenters hijos puedan hacer limpieza
        /// </summary>
        protected virtual void onFormClosing(object sender, FormClosingEventArgs e)
        {
            // Los hijos pueden sobreescribir para hacer limpieza
        }
    }
}
