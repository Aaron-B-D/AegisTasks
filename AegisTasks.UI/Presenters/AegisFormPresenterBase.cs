using AegisTasks.Core.Common;
using AegisTasks.UI.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AegisTasks.UI.Presenters
{
    public abstract class AegisFormPresenterBase<FormType> where FormType : Form
    {
        /// <summary>
        /// El formulario
        /// </summary>
        protected readonly FormType _Form;

        /// <summary>
        /// Inicializa el formulario
        /// </summary>
        public abstract void Initialize();

        public AegisFormPresenterBase(FormType view)
        {
            _Form = view ?? throw new ArgumentNullException(nameof(view));
            view.Load += onLoadHandler;
            view.FormClosing += onFormClosing;
        }

        /// <summary>
        /// Una validación que cada presenter debe implementar para establecer las condiciones en las que el formulario debe estar para ser permitida su carga. Por ejemplo si no se encuentra la sesión activa y por tanto no debería tenerse acceso al formulario
        /// </summary>
        /// <returns>True si se puede. False no</returns>
        protected abstract bool isLoadAllowed();


        /// <summary>
        /// Comprueba si en la app se encuentra una sesión activa
        /// </summary>
        protected bool isLogged()
        {
            return SessionManager.IsLoggedIn;
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
                _Form.Close();
                return;
            }

            try
            {
                this._Form.Icon = Images.Images.AegisLogo;
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
                _Form.Close();
            }
        }

        /// <summary>
        /// Método virtual para que los presenters hijos puedan hacer limpieza
        /// </summary>
        protected virtual void onFormClosing(object sender, FormClosingEventArgs e)
        {
            // Los hijos pueden sobreescribir para hacer limpieza
        }

        /// <summary>
        /// Muestra un mensaje de error estandarizado
        /// </summary>
        protected void showError(string message, string title)
        {
            MessageBox.Show(this._Form, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Muestra un mensaje de error estandarizado
        /// </summary>
        protected void showError(string message)
        {
            this.showError(message, Texts.Error);
        }

        /// <summary>
        /// Muestra un mensaje de información
        /// </summary>
        protected void showInfo(string message, string title)
        {
            MessageBox.Show(this._Form, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Muestra un diálogo de confirmación
        /// </summary>
        protected bool showConfirmation(string message, string title)
        {
            DialogResult result = MessageBox.Show(
                this._Form,
                message,
                title,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            return result == DialogResult.Yes;
        }

        /// <summary>
        /// Muestra un diálogo de advertencia
        /// </summary>
        protected void showWarn(string message, string title)
        {
            MessageBox.Show(
                this._Form,
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        /// <summary>
        /// Muestra un diálogo de advertencia
        /// </summary>
        protected void showWarn(string message)
        {
            this.showWarn(message, Texts.Warning);
        }
    }
}
