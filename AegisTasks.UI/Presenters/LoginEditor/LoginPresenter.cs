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

            this._View.LanguageComboBox.SelectedIndexChanged -= onLanguageChanged;
            this._View.LanguageComboBox.SelectedIndexChanged += onLanguageChanged;
            this._View.LoginButton.Click += onLoginButtonClicked;

            setLoginLanguage(LanguageEnum.SPANISH);
        }

        private void loadLanguagesIntoCombo()
        {
            this._View.LanguageComboBox.Items.Clear();

            this._View.LanguageComboBox.Items.Add(new LanguageItem(LanguageEnum.SPANISH, Texts.Spanish));
            this._View.LanguageComboBox.Items.Add(new LanguageItem(LanguageEnum.GALICIAN, Texts.Galician));
            this._View.LanguageComboBox.Items.Add(new LanguageItem(LanguageEnum.ENGLISH, Texts.English));
        }

        private void setLoginLanguage(LanguageEnum newLanguage)
        {
            // Cambiar cultura
            CultureInfo culture = newLanguage.ToCulture();
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Textos en la UI
            this._View.LoginButton.Text = Texts.Access;
            this._View.UserLabel.Text = Texts.User;
            this._View.PasswordLabel.Text = Texts.Password;
            this._View.Text = Texts.Login;
            this._View.LanguageLabel.Text = Texts.Language;

            // Seleccionar en el combo
            foreach (LanguageItem item in this._View.LanguageComboBox.Items)
            {
                if (item.Value == newLanguage)
                {
                    this._View.LanguageComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void onLanguageChanged(object sender, EventArgs e)
        {
            if (this._View.LanguageComboBox.SelectedItem is LanguageItem item)
            {
                setLoginLanguage(item.Value);
            }
        }

        private void onLoginButtonClicked(object sender, EventArgs e)
        {
            string user = this._View.UserTextbox.Text.ToLowerInvariant();
            string password = this._View.PasswordBox.Text;

            if(String.IsNullOrWhiteSpace(user)) {
                this.showWarn(Texts.IntroduceUser);
            }
            else if(String.IsNullOrWhiteSpace(password))
            {
                this.showWarn(Texts.IntroducePassword);
            }
            else if(SessionManager.Login(user, password))
            {
                this._View.Close();
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
