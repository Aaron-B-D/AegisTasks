namespace AegisTasks.UI.Forms
{
    partial class MainMenu
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
            this.OptionsStripMenu = new System.Windows.Forms.MenuStrip();
            this.TaskPlansOptionsStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.UsersOptionsStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.HistoryOptionsStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.LoginStripMenu = new System.Windows.Forms.MenuStrip();
            this.LogoutOptionLoginStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsStripMenu.SuspendLayout();
            this.LoginStripMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // OptionsStripMenu
            // 
            this.OptionsStripMenu.BackColor = System.Drawing.SystemColors.ControlLight;
            this.OptionsStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TaskPlansOptionsStripMenu,
            this.UsersOptionsStripMenu,
            this.HistoryOptionsStripMenu});
            this.OptionsStripMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.OptionsStripMenu.Location = new System.Drawing.Point(0, 0);
            this.OptionsStripMenu.Name = "OptionsStripMenu";
            this.OptionsStripMenu.Size = new System.Drawing.Size(375, 23);
            this.OptionsStripMenu.TabIndex = 0;
            this.OptionsStripMenu.Text = "menuStrip1";
            // 
            // TaskPlansOptionsStripMenu
            // 
            this.TaskPlansOptionsStripMenu.BackColor = System.Drawing.SystemColors.Menu;
            this.TaskPlansOptionsStripMenu.Name = "TaskPlansOptionsStripMenu";
            this.TaskPlansOptionsStripMenu.Size = new System.Drawing.Size(69, 19);
            this.TaskPlansOptionsStripMenu.Text = "TaskPlans";
            // 
            // UsersOptionsStripMenu
            // 
            this.UsersOptionsStripMenu.BackColor = System.Drawing.SystemColors.Menu;
            this.UsersOptionsStripMenu.Name = "UsersOptionsStripMenu";
            this.UsersOptionsStripMenu.Size = new System.Drawing.Size(47, 19);
            this.UsersOptionsStripMenu.Text = "Users";
            // 
            // HistoryOptionsStripMenu
            // 
            this.HistoryOptionsStripMenu.BackColor = System.Drawing.SystemColors.Menu;
            this.HistoryOptionsStripMenu.Name = "HistoryOptionsStripMenu";
            this.HistoryOptionsStripMenu.Size = new System.Drawing.Size(57, 19);
            this.HistoryOptionsStripMenu.Text = "History";
            // 
            // LoginStripMenu
            // 
            this.LoginStripMenu.BackColor = System.Drawing.SystemColors.MenuBar;
            this.LoginStripMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LoginStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LogoutOptionLoginStripMenu});
            this.LoginStripMenu.Location = new System.Drawing.Point(0, 206);
            this.LoginStripMenu.Name = "LoginStripMenu";
            this.LoginStripMenu.Size = new System.Drawing.Size(375, 24);
            this.LoginStripMenu.TabIndex = 1;
            this.LoginStripMenu.Text = "menuStrip2";
            // 
            // LogoutOptionLoginStripMenu
            // 
            this.LogoutOptionLoginStripMenu.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.LogoutOptionLoginStripMenu.BackColor = System.Drawing.SystemColors.Menu;
            this.LogoutOptionLoginStripMenu.Name = "LogoutOptionLoginStripMenu";
            this.LogoutOptionLoginStripMenu.Size = new System.Drawing.Size(57, 20);
            this.LogoutOptionLoginStripMenu.Text = "Logout";
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 230);
            this.Controls.Add(this.OptionsStripMenu);
            this.Controls.Add(this.LoginStripMenu);
            this.MainMenuStrip = this.OptionsStripMenu;
            this.Name = "MainMenu";
            this.Text = "MainMenu";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.OptionsStripMenu.ResumeLayout(false);
            this.OptionsStripMenu.PerformLayout();
            this.LoginStripMenu.ResumeLayout(false);
            this.LoginStripMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.MenuStrip OptionsStripMenu;
        public System.Windows.Forms.ToolStripMenuItem TaskPlansOptionsStripMenu;
        public System.Windows.Forms.ToolStripMenuItem UsersOptionsStripMenu;
        public System.Windows.Forms.ToolStripMenuItem HistoryOptionsStripMenu;
        public System.Windows.Forms.MenuStrip LoginStripMenu;
        public System.Windows.Forms.ToolStripMenuItem LogoutOptionLoginStripMenu;
    }
}