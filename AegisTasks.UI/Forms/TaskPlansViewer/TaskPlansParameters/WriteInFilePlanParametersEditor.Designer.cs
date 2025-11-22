namespace AegisTasks.UI.Forms
{
    partial class WriteInFilePlanParametersEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FilePathLabel = new System.Windows.Forms.Label();
            this.FilePathTextbox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.ContentLabel = new System.Windows.Forms.Label();
            this.ContentTextBox = new System.Windows.Forms.TextBox();
            this.AppendContentCheckBox = new System.Windows.Forms.CheckBox();
            this.FileToWriteDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // FilePathLabel
            // 
            this.FilePathLabel.AutoSize = true;
            this.FilePathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilePathLabel.Location = new System.Drawing.Point(6, 7);
            this.FilePathLabel.Name = "FilePathLabel";
            this.FilePathLabel.Size = new System.Drawing.Size(56, 13);
            this.FilePathLabel.TabIndex = 0;
            this.FilePathLabel.Text = "File path";
            // 
            // FilePathTextbox
            // 
            this.FilePathTextbox.Location = new System.Drawing.Point(126, 4);
            this.FilePathTextbox.Name = "FilePathTextbox";
            this.FilePathTextbox.ReadOnly = true;
            this.FilePathTextbox.Size = new System.Drawing.Size(651, 20);
            this.FilePathTextbox.TabIndex = 1;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(9, 34);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(768, 23);
            this.SearchButton.TabIndex = 2;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            // 
            // ContentLabel
            // 
            this.ContentLabel.AutoSize = true;
            this.ContentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContentLabel.Location = new System.Drawing.Point(6, 103);
            this.ContentLabel.Name = "ContentLabel";
            this.ContentLabel.Size = new System.Drawing.Size(51, 13);
            this.ContentLabel.TabIndex = 4;
            this.ContentLabel.Text = "Content";
            // 
            // ContentTextBox
            // 
            this.ContentTextBox.Location = new System.Drawing.Point(6, 119);
            this.ContentTextBox.Multiline = true;
            this.ContentTextBox.Name = "ContentTextBox";
            this.ContentTextBox.Size = new System.Drawing.Size(771, 314);
            this.ContentTextBox.TabIndex = 5;
            // 
            // AppendContentCheckBox
            // 
            this.AppendContentCheckBox.AutoSize = true;
            this.AppendContentCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AppendContentCheckBox.Location = new System.Drawing.Point(9, 63);
            this.AppendContentCheckBox.Name = "AppendContentCheckBox";
            this.AppendContentCheckBox.Size = new System.Drawing.Size(116, 17);
            this.AppendContentCheckBox.TabIndex = 3;
            this.AppendContentCheckBox.Text = "Append content";
            this.AppendContentCheckBox.UseVisualStyleBackColor = true;
            this.AppendContentCheckBox.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // WriteInFilePlanParametersEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AppendContentCheckBox);
            this.Controls.Add(this.ContentTextBox);
            this.Controls.Add(this.ContentLabel);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.FilePathTextbox);
            this.Controls.Add(this.FilePathLabel);
            this.Name = "WriteInFilePlanParametersEditor";
            this.Size = new System.Drawing.Size(783, 436);
            this.Load += new System.EventHandler(this.TaskPlanDetailsPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label FilePathLabel;
        public System.Windows.Forms.TextBox FilePathTextbox;
        public System.Windows.Forms.Button SearchButton;
        public System.Windows.Forms.Label ContentLabel;
        public System.Windows.Forms.TextBox ContentTextBox;
        public System.Windows.Forms.CheckBox AppendContentCheckBox;
        public System.Windows.Forms.OpenFileDialog FileToWriteDialog;
    }
}
