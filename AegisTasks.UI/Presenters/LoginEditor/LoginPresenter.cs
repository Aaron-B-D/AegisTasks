using AegisTasks.Core.Common;
using AegisTasks.UI.Common;
using AegisTasks.UI.Forms;
using AegisTasks.UI.Language;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AegisTasks.UI.Presenters
{
    using LanguageEnum = Core.Common.SupportedLanguage;

    public class LoginPresenter : AegisFormPresenterBase<Login>
    {
        public LoginPresenter(Login view) : base(view)
        {
        }

        public override void Initialize()
        {
            SessionManager.Logout();

            loadLanguagesIntoCombo();

            this._Form.LanguageComboBox.SelectedIndexChanged -= onLanguageChanged;
            this._Form.LanguageComboBox.SelectedIndexChanged += onLanguageChanged;
            this._Form.LoginButton.Click += onLoginButtonClicked;

            setLoginLanguage(LanguageEnum.SPANISH);
        }

        private void loadLanguagesIntoCombo()
        {
            this._Form.LanguageComboBox.Items.Clear();

            this._Form.LanguageComboBox.Items.Add(new LanguageItem(LanguageEnum.SPANISH, Texts.Spanish));
            this._Form.LanguageComboBox.Items.Add(new LanguageItem(LanguageEnum.GALICIAN, Texts.Galician));
            this._Form.LanguageComboBox.Items.Add(new LanguageItem(LanguageEnum.ENGLISH, Texts.English));
        }

        private void setLoginLanguage(LanguageEnum newLanguage)
        {
            // Cambiar cultura
            CultureInfo culture = newLanguage.ToCulture();
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Textos en la UI
            this._Form.LoginButton.Text = Texts.Access;
            this._Form.UserLabel.Text = Texts.User;
            this._Form.PasswordLabel.Text = Texts.Password;
            this._Form.Text = Texts.Login;
            this._Form.LanguageLabel.Text = Texts.Language;

            // Seleccionar en el combo
            foreach (LanguageItem item in this._Form.LanguageComboBox.Items)
            {
                if (item.Value == newLanguage)
                {
                    this._Form.LanguageComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void onLanguageChanged(object sender, EventArgs e)
        {
            if (this._Form.LanguageComboBox.SelectedItem is LanguageItem item)
            {
                setLoginLanguage(item.Value);
            }
        }

        private void onLoginButtonClicked(object sender, EventArgs e)
        {
            string user = this._Form.UserTextbox.Text.ToLowerInvariant();
            string password = this._Form.PasswordBox.Text;

            if(String.IsNullOrWhiteSpace(user)) {
                this.showWarn(Texts.IntroduceUser);
            }
            else if(String.IsNullOrWhiteSpace(password))
            {
                this.showWarn(Texts.IntroducePassword);
            }
            else if(SessionManager.Login(user, password))
            {
                this._Form.Close();
            }
            else
            {
                this.showError(Texts.AccessDenied);
            }
        }

        protected override bool isLoadAllowed()
        {
            return true;
        }
    }
}
