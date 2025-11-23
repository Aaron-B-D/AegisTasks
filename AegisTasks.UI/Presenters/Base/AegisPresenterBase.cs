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
    /// <summary>
    /// Clase base abstracta para presenters
    /// </summary>
    public abstract class AegisPresenterBase<ViewType> where ViewType : Control
    {
        /// <summary>
        /// La vista (Form o UserControl)
        /// </summary>
        protected readonly ViewType _View;

        /// <summary>
        /// Inicializa la vista
        /// </summary>
        public abstract void Initialize();

        public AegisPresenterBase(ViewType view)
        {
            _View = view ?? throw new ArgumentNullException(nameof(view));
        }

        /// <summary>
        /// Una validación que cada presenter debe implementar para establecer las condiciones en las que la vista debe estar para ser permitida su carga
        /// </summary>
        protected abstract bool isLoadAllowed();

        /// <summary>
        /// Comprueba si en la app se encuentra una sesión activa
        /// </summary>
        protected bool isLogged()
        {
            return SessionManager.IsLoggedIn;
        }

        /// <summary>
        /// Muestra un mensaje de error estandarizado
        /// </summary>
        protected void showError(string message, string title)
        {
            MessageBox.Show(_View, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Muestra un mensaje de error estandarizado
        /// </summary>
        protected void showError(string message)
        {
            showError(message, Texts.Error);
        }

        /// <summary>
        /// Muestra un mensaje de información
        /// </summary>
        protected void showInfo(string message, string title)
        {
            MessageBox.Show(_View, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Muestra un diálogo de confirmación
        /// </summary>
        protected bool showConfirmation(string message, string title)
        {
            DialogResult result = MessageBox.Show(
                _View,
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
            MessageBox.Show(_View, message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Muestra un diálogo de advertencia
        /// </summary>
        protected void showWarn(string message)
        {
            showWarn(message, Texts.Warning);
        }

        /// <summary>
        /// Muestra un mensaje de información
        /// </summary>
        protected void showInfo(string message)
        {
            showInfo(message, Texts.GeneralInfo);
        }
    }

}
