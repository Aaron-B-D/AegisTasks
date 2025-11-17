namespace AegisTasks.UI.Forms
{
    partial class UserEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.UsersSelectorGroupBox = new System.Windows.Forms.GroupBox();
            this.DeleteUserButton = new System.Windows.Forms.Button();
            this.AddUserButton = new System.Windows.Forms.Button();
            this.UsersList = new System.Windows.Forms.ListBox();
            this.UserDetailsGroupBox = new System.Windows.Forms.GroupBox();
            this.SaveChangesButton = new System.Windows.Forms.Button();
            this.UserDetailsUserParamsBox = new System.Windows.Forms.GroupBox();
            this.LanguageLabel = new System.Windows.Forms.Label();
            this.LanguageComboBox = new System.Windows.Forms.ComboBox();
            this.UserDetailsGeneralInfoBox = new System.Windows.Forms.GroupBox();
            this.SurnameLabel = new System.Windows.Forms.Label();
            this.ChangePasswordButton = new System.Windows.Forms.Button();
            this.UserLabel = new System.Windows.Forms.Label();
            this.SurnameTextBox = new System.Windows.Forms.TextBox();
            this.UserTextBox = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.UsersSelectorGroupBox.SuspendLayout();
            this.UserDetailsGroupBox.SuspendLayout();
            this.UserDetailsUserParamsBox.SuspendLayout();
            this.UserDetailsGeneralInfoBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // UsersSelectorGroupBox
            // 
            this.UsersSelectorGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.UsersSelectorGroupBox.Controls.Add(this.DeleteUserButton);
            this.UsersSelectorGroupBox.Controls.Add(this.AddUserButton);
            this.UsersSelectorGroupBox.Controls.Add(this.UsersList);
            this.UsersSelectorGroupBox.Location = new System.Drawing.Point(12, 12);
            this.UsersSelectorGroupBox.Name = "UsersSelectorGroupBox";
            this.UsersSelectorGroupBox.Size = new System.Drawing.Size(200, 426);
            this.UsersSelectorGroupBox.TabIndex = 1;
            this.UsersSelectorGroupBox.TabStop = false;
            this.UsersSelectorGroupBox.Text = "Available users";
            // 
            // DeleteUserButton
            // 
            this.DeleteUserButton.Location = new System.Drawing.Point(106, 398);
            this.DeleteUserButton.Name = "DeleteUserButton";
            this.DeleteUserButton.Size = new System.Drawing.Size(88, 23);
            this.DeleteUserButton.TabIndex = 2;
            this.DeleteUserButton.Text = "Delete";
            this.DeleteUserButton.UseVisualStyleBackColor = true;
            // 
            // AddUserButton
            // 
            this.AddUserButton.Location = new System.Drawing.Point(6, 397);
            this.AddUserButton.Name = "AddUserButton";
            this.AddUserButton.Size = new System.Drawing.Size(88, 23);
            this.AddUserButton.TabIndex = 1;
            this.AddUserButton.Text = "Add";
            this.AddUserButton.UseVisualStyleBackColor = true;
            // 
            // UsersList
            // 
            this.UsersList.FormattingEnabled = true;
            this.UsersList.Location = new System.Drawing.Point(6, 19);
            this.UsersList.Name = "UsersList";
            this.UsersList.Size = new System.Drawing.Size(188, 368);
            this.UsersList.TabIndex = 0;
            // 
            // UserDetailsGroupBox
            // 
            this.UserDetailsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserDetailsGroupBox.Controls.Add(this.SaveChangesButton);
            this.UserDetailsGroupBox.Controls.Add(this.UserDetailsUserParamsBox);
            this.UserDetailsGroupBox.Controls.Add(this.UserDetailsGeneralInfoBox);
            this.UserDetailsGroupBox.Location = new System.Drawing.Point(230, 12);
            this.UserDetailsGroupBox.Name = "UserDetailsGroupBox";
            this.UserDetailsGroupBox.Size = new System.Drawing.Size(558, 426);
            this.UserDetailsGroupBox.TabIndex = 2;
            this.UserDetailsGroupBox.TabStop = false;
            this.UserDetailsGroupBox.Text = "User details";
            // 
            // SaveChangesButton
            // 
            this.SaveChangesButton.Location = new System.Drawing.Point(9, 398);
            this.SaveChangesButton.Name = "SaveChangesButton";
            this.SaveChangesButton.Size = new System.Drawing.Size(543, 23);
            this.SaveChangesButton.TabIndex = 9;
            this.SaveChangesButton.Text = "Save changes";
            this.SaveChangesButton.UseVisualStyleBackColor = true;
            // 
            // UserDetailsUserParamsBox
            // 
            this.UserDetailsUserParamsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserDetailsUserParamsBox.Controls.Add(this.LanguageLabel);
            this.UserDetailsUserParamsBox.Controls.Add(this.LanguageComboBox);
            this.UserDetailsUserParamsBox.Location = new System.Drawing.Point(9, 203);
            this.UserDetailsUserParamsBox.Name = "UserDetailsUserParamsBox";
            this.UserDetailsUserParamsBox.Size = new System.Drawing.Size(543, 188);
            this.UserDetailsUserParamsBox.TabIndex = 8;
            this.UserDetailsUserParamsBox.TabStop = false;
            this.UserDetailsUserParamsBox.Text = "Parameters";
            // 
            // LanguageLabel
            // 
            this.LanguageLabel.AutoSize = true;
            this.LanguageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LanguageLabel.Location = new System.Drawing.Point(6, 25);
            this.LanguageLabel.Name = "LanguageLabel";
            this.LanguageLabel.Size = new System.Drawing.Size(63, 13);
            this.LanguageLabel.TabIndex = 2;
            this.LanguageLabel.Text = "Language";
            // 
            // LanguageComboBox
            // 
            this.LanguageComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LanguageComboBox.FormattingEnabled = true;
            this.LanguageComboBox.Location = new System.Drawing.Point(88, 22);
            this.LanguageComboBox.Name = "LanguageComboBox";
            this.LanguageComboBox.Size = new System.Drawing.Size(439, 21);
            this.LanguageComboBox.TabIndex = 3;
            // 
            // UserDetailsGeneralInfoBox
            // 
            this.UserDetailsGeneralInfoBox.Controls.Add(this.SurnameLabel);
            this.UserDetailsGeneralInfoBox.Controls.Add(this.ChangePasswordButton);
            this.UserDetailsGeneralInfoBox.Controls.Add(this.UserLabel);
            this.UserDetailsGeneralInfoBox.Controls.Add(this.SurnameTextBox);
            this.UserDetailsGeneralInfoBox.Controls.Add(this.UserTextBox);
            this.UserDetailsGeneralInfoBox.Controls.Add(this.NameLabel);
            this.UserDetailsGeneralInfoBox.Controls.Add(this.NameTextBox);
            this.UserDetailsGeneralInfoBox.Location = new System.Drawing.Point(9, 19);
            this.UserDetailsGeneralInfoBox.Name = "UserDetailsGeneralInfoBox";
            this.UserDetailsGeneralInfoBox.Size = new System.Drawing.Size(543, 177);
            this.UserDetailsGeneralInfoBox.TabIndex = 7;
            this.UserDetailsGeneralInfoBox.TabStop = false;
            this.UserDetailsGeneralInfoBox.Text = "General info";
            // 
            // SurnameLabel
            // 
            this.SurnameLabel.AutoSize = true;
            this.SurnameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SurnameLabel.Location = new System.Drawing.Point(6, 106);
            this.SurnameLabel.Name = "SurnameLabel";
            this.SurnameLabel.Size = new System.Drawing.Size(56, 13);
            this.SurnameLabel.TabIndex = 4;
            this.SurnameLabel.Text = "Surname";
            // 
            // ChangePasswordButton
            // 
            this.ChangePasswordButton.Location = new System.Drawing.Point(9, 139);
            this.ChangePasswordButton.Name = "ChangePasswordButton";
            this.ChangePasswordButton.Size = new System.Drawing.Size(528, 23);
            this.ChangePasswordButton.TabIndex = 6;
            this.ChangePasswordButton.Text = "Change password";
            this.ChangePasswordButton.UseVisualStyleBackColor = true;
            // 
            // UserLabel
            // 
            this.UserLabel.AutoSize = true;
            this.UserLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserLabel.Location = new System.Drawing.Point(6, 24);
            this.UserLabel.Name = "UserLabel";
            this.UserLabel.Size = new System.Drawing.Size(33, 13);
            this.UserLabel.TabIndex = 0;
            this.UserLabel.Text = "User";
            // 
            // SurnameTextBox
            // 
            this.SurnameTextBox.Location = new System.Drawing.Point(88, 103);
            this.SurnameTextBox.Name = "SurnameTextBox";
            this.SurnameTextBox.Size = new System.Drawing.Size(449, 20);
            this.SurnameTextBox.TabIndex = 5;
            // 
            // UserTextBox
            // 
            this.UserTextBox.Location = new System.Drawing.Point(88, 21);
            this.UserTextBox.Name = "UserTextBox";
            this.UserTextBox.Size = new System.Drawing.Size(449, 20);
            this.UserTextBox.TabIndex = 1;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.Location = new System.Drawing.Point(6, 64);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(39, 13);
            this.NameLabel.TabIndex = 2;
            this.NameLabel.Text = "Name";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(88, 61);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(449, 20);
            this.NameTextBox.TabIndex = 3;
            this.NameTextBox.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // UserEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.UserDetailsGroupBox);
            this.Controls.Add(this.UsersSelectorGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "UserEditor";
            this.Text = "UserEditor";
            this.Load += new System.EventHandler(this.UserEditor_Load);
            this.UsersSelectorGroupBox.ResumeLayout(false);
            this.UserDetailsGroupBox.ResumeLayout(false);
            this.UserDetailsUserParamsBox.ResumeLayout(false);
            this.UserDetailsUserParamsBox.PerformLayout();
            this.UserDetailsGeneralInfoBox.ResumeLayout(false);
            this.UserDetailsGeneralInfoBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox UsersSelectorGroupBox;
        public System.Windows.Forms.ListBox UsersList;
        public System.Windows.Forms.GroupBox UserDetailsUserParamsBox;
        public System.Windows.Forms.GroupBox UserDetailsGeneralInfoBox;
        public System.Windows.Forms.TextBox SurnameTextBox;
        public System.Windows.Forms.Label SurnameLabel;
        public System.Windows.Forms.TextBox NameTextBox;
        public System.Windows.Forms.Label NameLabel;
        public System.Windows.Forms.TextBox UserTextBox;
        public System.Windows.Forms.Label UserLabel;
        public System.Windows.Forms.Button ChangePasswordButton;
        public System.Windows.Forms.Label LanguageLabel;
        public System.Windows.Forms.ComboBox LanguageComboBox;
        public System.Windows.Forms.Button SaveChangesButton;
        public System.Windows.Forms.Button DeleteUserButton;
        public System.Windows.Forms.Button AddUserButton;
        public System.Windows.Forms.GroupBox UserDetailsGroupBox;
    }
}