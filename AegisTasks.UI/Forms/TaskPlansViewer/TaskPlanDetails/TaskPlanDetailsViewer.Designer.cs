namespace AegisTasks.UI.Forms
{
    partial class TaskPlanDetailsViewer
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
            this.DetailsContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.GeneralInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameTextbox = new System.Windows.Forms.TextBox();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.DescriptionTextbox = new System.Windows.Forms.TextBox();
            this.ParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.SaveAsTemplateButton = new System.Windows.Forms.Button();
            this.ExecuteButton = new System.Windows.Forms.Button();
            this.ButtonsContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.DetailsContainer.SuspendLayout();
            this.GeneralInfoGroupBox.SuspendLayout();
            this.ButtonsContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // DetailsContainer
            // 
            this.DetailsContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DetailsContainer.Controls.Add(this.GeneralInfoGroupBox);
            this.DetailsContainer.Controls.Add(this.ParametersGroupBox);
            this.DetailsContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.DetailsContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.DetailsContainer.Location = new System.Drawing.Point(0, 0);
            this.DetailsContainer.Name = "DetailsContainer";
            this.DetailsContainer.Size = new System.Drawing.Size(793, 609);
            this.DetailsContainer.TabIndex = 1;
            // 
            // GeneralInfoGroupBox
            // 
            this.GeneralInfoGroupBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.GeneralInfoGroupBox.Controls.Add(this.NameLabel);
            this.GeneralInfoGroupBox.Controls.Add(this.NameTextbox);
            this.GeneralInfoGroupBox.Controls.Add(this.DescriptionLabel);
            this.GeneralInfoGroupBox.Controls.Add(this.DescriptionTextbox);
            this.GeneralInfoGroupBox.Location = new System.Drawing.Point(3, 3);
            this.GeneralInfoGroupBox.Name = "GeneralInfoGroupBox";
            this.GeneralInfoGroupBox.Size = new System.Drawing.Size(787, 141);
            this.GeneralInfoGroupBox.TabIndex = 0;
            this.GeneralInfoGroupBox.TabStop = false;
            this.GeneralInfoGroupBox.Text = "General info";
            this.GeneralInfoGroupBox.Enter += new System.EventHandler(this.GeneralInfoGroupBox_Enter);
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.Location = new System.Drawing.Point(18, 22);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(39, 13);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Name";
            // 
            // NameTextbox
            // 
            this.NameTextbox.Location = new System.Drawing.Point(76, 19);
            this.NameTextbox.Name = "NameTextbox";
            this.NameTextbox.ReadOnly = true;
            this.NameTextbox.Size = new System.Drawing.Size(705, 22);
            this.NameTextbox.TabIndex = 1;
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DescriptionLabel.Location = new System.Drawing.Point(18, 53);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(71, 13);
            this.DescriptionLabel.TabIndex = 2;
            this.DescriptionLabel.Text = "Description";
            // 
            // DescriptionTextbox
            // 
            this.DescriptionTextbox.Location = new System.Drawing.Point(21, 72);
            this.DescriptionTextbox.Multiline = true;
            this.DescriptionTextbox.Name = "DescriptionTextbox";
            this.DescriptionTextbox.ReadOnly = true;
            this.DescriptionTextbox.Size = new System.Drawing.Size(760, 63);
            this.DescriptionTextbox.TabIndex = 3;
            // 
            // ParametersGroupBox
            // 
            this.ParametersGroupBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ParametersGroupBox.Location = new System.Drawing.Point(3, 150);
            this.ParametersGroupBox.Name = "ParametersGroupBox";
            this.ParametersGroupBox.Size = new System.Drawing.Size(787, 456);
            this.ParametersGroupBox.TabIndex = 1;
            this.ParametersGroupBox.TabStop = false;
            this.ParametersGroupBox.Text = "Parameters";
            this.ParametersGroupBox.Enter += new System.EventHandler(this.ParametersGroupBox_Enter);
            // 
            // SaveAsTemplateButton
            // 
            this.SaveAsTemplateButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SaveAsTemplateButton.Location = new System.Drawing.Point(384, 3);
            this.SaveAsTemplateButton.Name = "SaveAsTemplateButton";
            this.SaveAsTemplateButton.Size = new System.Drawing.Size(200, 30);
            this.SaveAsTemplateButton.TabIndex = 0;
            this.SaveAsTemplateButton.Text = "Save as template";
            this.SaveAsTemplateButton.UseVisualStyleBackColor = true;
            this.SaveAsTemplateButton.Click += new System.EventHandler(this.SaveAsTemplateButton_Click);
            // 
            // ExecuteButton
            // 
            this.ExecuteButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ExecuteButton.Location = new System.Drawing.Point(590, 3);
            this.ExecuteButton.Name = "ExecuteButton";
            this.ExecuteButton.Size = new System.Drawing.Size(200, 30);
            this.ExecuteButton.TabIndex = 1;
            this.ExecuteButton.Text = "Execute";
            this.ExecuteButton.UseVisualStyleBackColor = true;
            // 
            // ButtonsContainer
            // 
            this.ButtonsContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ButtonsContainer.Controls.Add(this.ExecuteButton);
            this.ButtonsContainer.Controls.Add(this.SaveAsTemplateButton);
            this.ButtonsContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ButtonsContainer.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.ButtonsContainer.Location = new System.Drawing.Point(0, 612);
            this.ButtonsContainer.Name = "ButtonsContainer";
            this.ButtonsContainer.Size = new System.Drawing.Size(793, 40);
            this.ButtonsContainer.TabIndex = 2;
            this.ButtonsContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonsContainer_Paint);
            // 
            // TaskPlanDetailsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DetailsContainer);
            this.Controls.Add(this.ButtonsContainer);
            this.Name = "TaskPlanDetailsViewer";
            this.Size = new System.Drawing.Size(793, 652);
            this.DetailsContainer.ResumeLayout(false);
            this.GeneralInfoGroupBox.ResumeLayout(false);
            this.GeneralInfoGroupBox.PerformLayout();
            this.ButtonsContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel DetailsContainer;
        private System.Windows.Forms.FlowLayoutPanel ButtonsContainer;
        public System.Windows.Forms.Button SaveAsTemplateButton;
        public System.Windows.Forms.Button ExecuteButton;
        public System.Windows.Forms.GroupBox GeneralInfoGroupBox;
        public System.Windows.Forms.GroupBox ParametersGroupBox;
        public System.Windows.Forms.TextBox DescriptionTextbox;
        public System.Windows.Forms.Label DescriptionLabel;
        public System.Windows.Forms.TextBox NameTextbox;
        public System.Windows.Forms.Label NameLabel;
    }
}
