using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.Common;
using AegisTasks.Core.DTO;
using AegisTasks.UI.Common;
using AegisTasks.UI.Forms;
using AegisTasks.UI.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace AegisTasks.UI.Presenters
{
    public class UserEditorPresenter : AegisFormPresenterBase<UserEditor>
    {
        private BindingList<UserDTO> _Users = new BindingList<UserDTO>();
        private UserDTO _User = null;
        private UserParametersDTO _UserParameters = null;
        private BindingSource _UsersBindingSource = new BindingSource();

        public UserEditorPresenter(UserEditor view) : base(view) { }

        public override void Initialize()
        {
            List<UserDTO> usersFromDb = UserDataAccessBLL.Get();
            if (usersFromDb == null)
            {
                usersFromDb = new List<UserDTO>();
            }

            _Users = new BindingList<UserDTO>(usersFromDb);
            _UsersBindingSource.DataSource = _Users;

            loadLanguagesIntoCombo();

            _Form.Text = Texts.UserEditor;
            _Form.UsersSelectorGroupBox.Text = Texts.AvailableUsers;
            _Form.UserDetailsGeneralInfoBox.Text = Texts.UserDetails;
            _Form.AddUserButton.Text = Texts.Add;
            _Form.DeleteUserButton.Text = Texts.Delete;
            _Form.SaveChangesButton.Text = Texts.SaveChanges;
            _Form.UserLabel.Text = Texts.User;
            _Form.NameLabel.Text = Texts.Name;
            _Form.SurnameLabel.Text = Texts.Surname;
            _Form.ChangePasswordButton.Text = Texts.ChangePassword;
            _Form.LanguageLabel.Text = Texts.Language;
            _Form.UserDetailsUserParamsBox.Text = Texts.Parameters;
            _Form.UserDetailsGroupBox.Text = Texts.UserDetails;

            _Form.UsersList.DisplayMember = "Username";
            _Form.UsersList.DataSource = _UsersBindingSource;

            _Form.UsersList.SelectedIndexChanged += onUserSelected;
            _Form.AddUserButton.Click += onAddUserClicked;
            _Form.DeleteUserButton.Click += onDeleteUserClicked;
            _Form.SaveChangesButton.Click += onSaveChangesClicked;
            _Form.ChangePasswordButton.Click += onChangePasswordClicked;

            _Form.NameTextBox.TextChanged += onUserDetailChanged;
            _Form.SurnameTextBox.TextChanged += onUserDetailChanged;
            _Form.LanguageComboBox.SelectedIndexChanged += onUserDetailChanged;

            _User = _Users.FirstOrDefault(u =>
            {
                return u.Username.Equals(SessionManager.CurrentUser.Username,
                    StringComparison.InvariantCultureIgnoreCase);
            });

            if (_User != null)
            {
                _Form.UsersList.SelectedItem = _User;
                loadUserIntoForm(_User);
            }
            else
            {
                if (_Users.Count > 0)
                {
                    _Form.UsersList.SelectedIndex = 0;
                    _User = (UserDTO)_Form.UsersList.SelectedItem;
                    loadUserIntoForm(_User);
                }
            }

            updateButtonsState();
        }

        protected override bool isLoadAllowed()
        {
            return isLogged();
        }

        private void onUserSelected(object sender, EventArgs e)
        {
            UserDTO selected = _Form.UsersList.SelectedItem as UserDTO;
            if (selected != null)
            {
                _User = selected;
                loadUserIntoForm(_User);
            }
            updateButtonsState();
        }

        private void onAddUserClicked(object sender, EventArgs e)
        {
            using (AddUserEditor addForm = new AddUserEditor())
            {
                addForm.FormClosed += (s, args) =>
                {
                    // recargar lista de usuarios desde BLL
                    _Users.Clear();
                    foreach (UserDTO u in UserDataAccessBLL.Get())
                    {
                        _Users.Add(u);
                    }

                    if (_Users.Count > 0)
                    {
                        _User = _Users[0];
                        _Form.UsersList.SelectedItem = _User;
                        loadUserIntoForm(_User);
                    }

                    updateButtonsState();
                };

                addForm.ShowDialog(this._Form);
            }
        }


        private void onDeleteUserClicked(object sender, EventArgs e)
        {
            if (_User != null)
            {
                if (_User.Username.Equals(UserDTO.ADMIN_USER_USERNAME,
                    StringComparison.InvariantCultureIgnoreCase)) {
                    this.showWarn(Texts.AdminUserCannotBeErased);
                }
                else if (_User.Username.Equals(SessionManager.CurrentUser.Username,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    this.showWarn(Texts.UserCannotEraseThemselves);
                }

                else
                {
                    UserDataAccessBLL.DeleteUser(_User.Username);
                    _Users.Remove(_User);
                    _User = _Users.FirstOrDefault();

                    if (_User != null)
                    {
                        _Form.UsersList.SelectedItem = _User;
                        loadUserIntoForm(_User);
                    }
                }
            }

            updateButtonsState();
        }

        private void onChangePasswordClicked(object sender, EventArgs e)
        {
            using (ChangePasswordEditor changePasswordForm = new ChangePasswordEditor(_User))
            {
                changePasswordForm.ShowDialog(this._Form);
            }
        }

        private void onSaveChangesClicked(object sender, EventArgs e)
        {
            if (_User != null)
            {
                string firstName = _Form.NameTextBox.Text;
                string lastName = _Form.SurnameTextBox.Text;

                bool updated = UserDataAccessBLL.UpdateUserInfo(_User.Username, firstName, lastName);
                if (updated)
                {
                    _User.FirstName = firstName;
                    _User.LastName = lastName;

                    if (_Form.LanguageComboBox.SelectedItem is LanguageItem item)
                    {
                        UserParameterDTO<object> param =
                            new UserParameterDTO<object>(UserParameterType.LANGUAGE, item.Value);
                        UserParametersBLL.ModifyParameter(_User.Username, param);
                    }

                    MessageBox.Show(Texts.SavedSuccessfully);
                }
                else
                {
                    MessageBox.Show(Texts.SaveError);
                }
            }

            updateButtonsState();
        }

        private void onUserDetailChanged(object sender, EventArgs e)
        {
            updateButtonsState();
        }

        private void loadUserIntoForm(UserDTO user)
        {
            if (user != null)
            {
                _UserParameters = UserParametersBLL.GetParameters(user.Username);

                _Form.UserTextBox.Text = user.Username;
                _Form.NameTextBox.Text = user.FirstName;
                _Form.SurnameTextBox.Text = user.LastName;

                if (_UserParameters != null && _UserParameters.TryGetParameter<SupportedLanguage>(UserParameterType.LANGUAGE, out UserParameterDTO<SupportedLanguage> lang))
                {
                    selectLanguageInCombo(lang.Value);
                }
                else
                {
                    _Form.LanguageComboBox.SelectedIndex = 0;
                }
            }
            else
            {
                _Form.UserTextBox.Text = "";
                _Form.NameTextBox.Text = "";
                _Form.SurnameTextBox.Text = "";
                _Form.LanguageComboBox.SelectedIndex = -1;
            }
        }

        private void loadLanguagesIntoCombo()
        {
            _Form.LanguageComboBox.Items.Clear();

            _Form.LanguageComboBox.Items.Add(new LanguageItem(SupportedLanguage.SPANISH, Texts.Spanish));
            _Form.LanguageComboBox.Items.Add(new LanguageItem(SupportedLanguage.GALICIAN, Texts.Galician));
            _Form.LanguageComboBox.Items.Add(new LanguageItem(SupportedLanguage.ENGLISH, Texts.English));
        }

        private void selectLanguageInCombo(SupportedLanguage language)
        {
            foreach (object obj in _Form.LanguageComboBox.Items)
            {
                LanguageItem langItem = obj as LanguageItem;
                if (langItem != null)
                {
                    if (langItem.Value == language)
                    {
                        _Form.LanguageComboBox.SelectedItem = langItem;
                    }
                }
            }
        }

        private bool hasChanges(UserDTO original)
        {
            bool nameChanged = _Form.NameTextBox.Text != original.FirstName;
            bool surnameChanged = _Form.SurnameTextBox.Text != original.LastName;
            LanguageItem selected = _Form.LanguageComboBox.SelectedItem as LanguageItem;
            bool languageChanged = false;

            if (_UserParameters != null && selected != null && _UserParameters.TryGetParameter<SupportedLanguage>(UserParameterType.LANGUAGE, out UserParameterDTO<SupportedLanguage> param))
            {
                languageChanged = selected.Value != param.Value;
            }

            return nameChanged || surnameChanged || languageChanged;
        }

        private void updateButtonsState()
        {
            if (_User == null)
            {
                _Form.DeleteUserButton.Enabled = false;
                _Form.SaveChangesButton.Enabled = false;
            }
            else
            {
                _Form.DeleteUserButton.Enabled = true;
                _Form.SaveChangesButton.Enabled = hasChanges(_User);
            }
        }
    }
}
