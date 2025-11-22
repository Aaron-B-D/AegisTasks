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
            string username = this._View.UserTextBox.Text.Trim();
            string firstName = this._View.NameTextBox.Text.Trim();
            string lastName = this._View.SurnameTextBox.Text.Trim();
            string password = this._View.PasswordTextBox.Text;
            string confirmPassword = this._View.ConfirmPasswordTextBox.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show(Texts.IntroduceUser);
            }
            else if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show(Texts.IntroducePassword);
            }
            else if (password != confirmPassword)
            {
                MessageBox.Show(Texts.PasswordsDoNotMatch);
            }
            else
            {
                // Validar que el usuario no exista
                if (UserDataAccessBLL.GetUser(username) != null)
                {
                    MessageBox.Show(Texts.UserExists);
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
                        MessageBox.Show(Texts.UserCreated);
                        this._View.DialogResult = DialogResult.OK;
                        this._View.Close();
                    }
                    else
                    {
                        MessageBox.Show(Texts.SaveError);
                    }
                }
            }
        }

    }
}
