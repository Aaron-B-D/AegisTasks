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
            this._Form.Text = Texts.ChangePassword;
            this._Form.OldPasswordLabel.Text = Texts.OldPassword;
            this._Form.NewPasswordLabel.Text = Texts.NewPassword;
            this._Form.ConfirmPasswordLabel.Text = Texts.ConfirmPassword;
            this._Form.SaveButton.Text = Texts.Save;

            this._Form.SaveButton.Click -= onSaveClicked;
            this._Form.SaveButton.Click += onSaveClicked;
        }

        protected override bool isLoadAllowed()
        {
            return this.isLogged();
        }

        private void onSaveClicked(object sender, EventArgs e)
        {
            string oldPassword = this._Form.OldPasswordTextBox.Text;
            string newPassword = this._Form.NewPasswordTextBox.Text;
            string confirmPassword = this._Form.ConfirmPasswordTextBox.Text;

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
                            this._Form.DialogResult = DialogResult.OK;
                            this._Form.Close();
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
