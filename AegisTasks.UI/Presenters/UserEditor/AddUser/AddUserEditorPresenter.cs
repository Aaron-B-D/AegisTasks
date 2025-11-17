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
            this._Form.Text = Texts.AddNewUser;
            this._Form.UserLabel.Text = Texts.User;
            this._Form.NameLabel.Text = Texts.Name;
            this._Form.SurnameLabel.Text = Texts.Surname;
            this._Form.PasswordLabel.Text = Texts.Password;
            this._Form.ConfirmPasswordLabel.Text = Texts.ConfirmPassword;
            this._Form.SaveButton.Text = Texts.Save;

            this._Form.SaveButton.Click += onSaveClicked;
        }

        protected override bool isLoadAllowed()
        {
            return this.isLogged();
        }

        private void onSaveClicked(object sender, EventArgs e)
        {
            string username = this._Form.UserTextBox.Text.Trim();
            string firstName = this._Form.NameTextBox.Text.Trim();
            string lastName = this._Form.SurnameTextBox.Text.Trim();
            string password = this._Form.PasswordTextBox.Text;
            string confirmPassword = this._Form.ConfirmPasswordTextBox.Text;

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
                        this._Form.DialogResult = DialogResult.OK;
                        this._Form.Close();
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
