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

            _View.Text = Texts.UserEditor;
            _View.UsersSelectorGroupBox.Text = Texts.AvailableUsers;
            _View.UserDetailsGeneralInfoBox.Text = Texts.GeneralInfo;
            _View.AddUserButton.Text = Texts.Add;
            _View.DeleteUserButton.Text = Texts.Delete;
            _View.SaveChangesButton.Text = Texts.SaveChanges;
            _View.UserLabel.Text = Texts.User;
            _View.NameLabel.Text = Texts.Name;
            _View.SurnameLabel.Text = Texts.Surname;
            _View.ChangePasswordButton.Text = Texts.ChangePassword;
            _View.LanguageLabel.Text = Texts.Language;
            _View.UserDetailsUserParamsBox.Text = Texts.Parameters;
            _View.UserDetailsGroupBox.Text = Texts.UserDetails;

            _View.UsersList.DisplayMember = "Username";
            _View.UsersList.DataSource = _UsersBindingSource;

            _View.UsersList.SelectedIndexChanged += onUserSelected;
            _View.AddUserButton.Click += onAddUserClicked;
            _View.DeleteUserButton.Click += onDeleteUserClicked;
            _View.SaveChangesButton.Click += onSaveChangesClicked;
            _View.ChangePasswordButton.Click += onChangePasswordClicked;

            _View.NameTextBox.TextChanged += onUserDetailChanged;
            _View.SurnameTextBox.TextChanged += onUserDetailChanged;
            _View.LanguageComboBox.SelectedIndexChanged += onUserDetailChanged;

            _User = _Users.FirstOrDefault(u =>
            {
                return u.Username.Equals(SessionManager.CurrentUser.Username,
                    StringComparison.InvariantCultureIgnoreCase);
            });

            if (_User != null)
            {
                _View.UsersList.SelectedItem = _User;
                loadUserIntoForm(_User);
            }
            else
            {
                if (_Users.Count > 0)
                {
                    _View.UsersList.SelectedIndex = 0;
                    _User = (UserDTO)_View.UsersList.SelectedItem;
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
            UserDTO selected = _View.UsersList.SelectedItem as UserDTO;
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
                        _View.UsersList.SelectedItem = _User;
                        loadUserIntoForm(_User);
                    }

                    updateButtonsState();
                };

                addForm.ShowDialog(this._View);
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
                        _View.UsersList.SelectedItem = _User;
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
                changePasswordForm.ShowDialog(this._View);
            }
        }

        private void onSaveChangesClicked(object sender, EventArgs e)
        {
            if (_User != null)
            {
                string firstName = _View.NameTextBox.Text;
                string lastName = _View.SurnameTextBox.Text;

                bool updated = UserDataAccessBLL.UpdateUserInfo(_User.Username, firstName, lastName);
                if (updated)
                {
                    _User.FirstName = firstName;
                    _User.LastName = lastName;

                    if (_View.LanguageComboBox.SelectedItem is LanguageItem item)
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

                _View.UserTextBox.Text = user.Username;
                _View.NameTextBox.Text = user.FirstName;
                _View.SurnameTextBox.Text = user.LastName;

                if (_UserParameters != null && _UserParameters.TryGetParameter<SupportedLanguage>(UserParameterType.LANGUAGE, out UserParameterDTO<SupportedLanguage> lang))
                {
                    selectLanguageInCombo(lang.Value);
                }
                else
                {
                    _View.LanguageComboBox.SelectedIndex = 0;
                }
            }
            else
            {
                _View.UserTextBox.Text = "";
                _View.NameTextBox.Text = "";
                _View.SurnameTextBox.Text = "";
                _View.LanguageComboBox.SelectedIndex = -1;
            }
        }

        private void loadLanguagesIntoCombo()
        {
            _View.LanguageComboBox.Items.Clear();

            _View.LanguageComboBox.Items.Add(new LanguageItem(SupportedLanguage.SPANISH, Texts.Spanish));
            _View.LanguageComboBox.Items.Add(new LanguageItem(SupportedLanguage.GALICIAN, Texts.Galician));
            _View.LanguageComboBox.Items.Add(new LanguageItem(SupportedLanguage.ENGLISH, Texts.English));
        }

        private void selectLanguageInCombo(SupportedLanguage language)
        {
            foreach (object obj in _View.LanguageComboBox.Items)
            {
                LanguageItem langItem = obj as LanguageItem;
                if (langItem != null)
                {
                    if (langItem.Value == language)
                    {
                        _View.LanguageComboBox.SelectedItem = langItem;
                    }
                }
            }
        }

        private bool hasChanges(UserDTO original)
        {
            bool nameChanged = _View.NameTextBox.Text != original.FirstName;
            bool surnameChanged = _View.SurnameTextBox.Text != original.LastName;
            LanguageItem selected = _View.LanguageComboBox.SelectedItem as LanguageItem;
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
                _View.DeleteUserButton.Enabled = false;
                _View.SaveChangesButton.Enabled = false;
            }
            else
            {
                _View.DeleteUserButton.Enabled = true;
                _View.SaveChangesButton.Enabled = hasChanges(_User);
            }
        }
    }
}
