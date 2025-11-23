using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.Common;
using AegisTasks.Core.DTO;
using AegisTasks.UI.Forms;
using AegisTasks.UI.Language;
using System;
using System.Windows.Forms;

namespace AegisTasks.UI.Presenters
{
    public class AddUserEditorPresenter : AegisFormPresenterBase<AddUserEditor>
    {
        public AddUserEditorPresenter(AddUserEditor view) : base(view)
        {
        }

        public override void Initialize()
        {
            this._View.Text = Texts.AddNewUser;
            this._View.UserLabel.Text = Texts.User;
            this._View.NameLabel.Text = Texts.Name;
            this._View.SurnameLabel.Text = Texts.Surname;
            this._View.PasswordLabel.Text = Texts.Password;
            this._View.ConfirmPasswordLabel.Text = Texts.ConfirmPassword;
            this._View.SaveButton.Text = Texts.Save;

            this._View.SaveButton.Click += onSaveClicked;
        }

        protected override bool isLoadAllowed()
        {
            return this.isLogged();
        }

        private void onSaveClicked(object sender, EventArgs e)
        {
            try
            {
                string username = this._View.UserTextBox.Text.Trim();
                string firstName = this._View.NameTextBox.Text.Trim();
                string lastName = this._View.SurnameTextBox.Text.Trim();
                string password = this._View.PasswordTextBox.Text;
                string confirmPassword = this._View.ConfirmPasswordTextBox.Text;

                if (string.IsNullOrEmpty(username))
                {
                    this.showWarn(Texts.IntroduceUser);
                }
                else if (string.IsNullOrEmpty(password))
                {
                    this.showWarn(Texts.IntroducePassword);
                }
                else if (password != confirmPassword)
                {
                    this.showWarn(Texts.PasswordsDoNotMatch);
                }
                else
                {
                    // Validar que el usuario no exista
                    if (UserDataAccessBLL.GetUser(username) != null)
                    {
                        this.showWarn(Texts.UserExists);
                    }
                    else
                    {
                        UserDTO newUser = new UserDTO()
                        {
                            Username = username,
                            FirstName = firstName,
                            LastName = lastName,
                            Password = password
                        };

                        bool created = UserDataAccessBLL.CreateUser(newUser);
                        if (created)
                        {
                            this.showInfo(Texts.UserCreated);
                            this._View.DialogResult = DialogResult.OK;
                            this._View.Close();
                        }
                        else
                        {
                            this.showError(Texts.SaveError);
                        }
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Logger.LogException(ex);
                this.showWarn(Texts.PasswordDoNotMeetCriteria);
            }
            catch (Exception ex) {
                Logger.LogException(ex);
                this.showError(Texts.SaveError);
            }
        }

    }
}
