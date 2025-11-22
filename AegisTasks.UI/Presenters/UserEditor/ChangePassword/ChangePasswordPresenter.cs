using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.DTO;
using AegisTasks.UI.Forms;
using AegisTasks.UI.Language;
using System;
using System.Windows.Forms;

namespace AegisTasks.UI.Presenters
{
    public class ChangePasswordPresenter : AegisFormPresenterBase<ChangePasswordEditor>
    {
        private UserDTO _User;

        public ChangePasswordPresenter(ChangePasswordEditor view, UserDTO user) : base(view)
        {
            _User = user;
        }

        public override void Initialize()
        {
            this._View.Text = Texts.ChangePassword;
            this._View.OldPasswordLabel.Text = Texts.OldPassword;
            this._View.NewPasswordLabel.Text = Texts.NewPassword;
            this._View.ConfirmPasswordLabel.Text = Texts.ConfirmPassword;
            this._View.SaveButton.Text = Texts.Save;

            this._View.SaveButton.Click -= onSaveClicked;
            this._View.SaveButton.Click += onSaveClicked;
        }

        protected override bool isLoadAllowed()
        {
            return this.isLogged();
        }

        private void onSaveClicked(object sender, EventArgs e)
        {
            string oldPassword = this._View.OldPasswordTextBox.Text;
            string newPassword = this._View.NewPasswordTextBox.Text;
            string confirmPassword = this._View.ConfirmPasswordTextBox.Text;

            if (string.IsNullOrEmpty(oldPassword))
            {
                MessageBox.Show(Texts.IntroduceOldPassword);
            }
            else if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show(Texts.IntroduceNewPassword);
            }
            else if (newPassword != confirmPassword)
            {
                MessageBox.Show(Texts.PasswordsDoNotMatch);
            }
            else
            {
                try
                {
                    if(UserDataAccessBLL.IsValidPassword(_User.Username, oldPassword))
                    {
                        // UpdatePassword espera el username, la contraseña antigua y la nueva
                        bool changed = UserDataAccessBLL.UpdatePassword(_User.Username, oldPassword, newPassword);

                        if (changed)
                        {
                            MessageBox.Show(Texts.PasswordChangesSuccess);
                            this._View.DialogResult = DialogResult.OK;
                            this._View.Close();
                        }
                        else
                        {
                            MessageBox.Show(Texts.PasswordChangesError);

                        }
                    }
                    else
                    {
                        MessageBox.Show(Texts.OldPasswordIncorrect);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Texts.SaveError + "\n" + ex.Message);
                }
            }
        }
    }
}
