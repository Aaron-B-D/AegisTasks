namespace AegisTasks.UI.Forms
{
    partial class TaskPlansViewer
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
            this.TaskActionGroupBox = new System.Windows.Forms.GroupBox();
            this.TaskActionsList = new System.Windows.Forms.ListBox();
            this.TemplatesGroupBox = new System.Windows.Forms.GroupBox();
            this.TemplatesList = new System.Windows.Forms.ListBox();
            this.DetailsPanel = new System.Windows.Forms.Panel();
            this.SelectorContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.TaskActionGroupBox.SuspendLayout();
            this.TemplatesGroupBox.SuspendLayout();
            this.SelectorContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // TaskActionGroupBox
            // 
            this.TaskActionGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TaskActionGroupBox.Controls.Add(this.TaskActionsList);
            this.TaskActionGroupBox.Location = new System.Drawing.Point(3, 3);
            this.TaskActionGroupBox.Name = "TaskActionGroupBox";
            this.TaskActionGroupBox.Size = new System.Drawing.Size(192, 320);
            this.TaskActionGroupBox.TabIndex = 0;
            this.TaskActionGroupBox.TabStop = false;
            this.TaskActionGroupBox.Text = "Action plans";
            // 
            // TaskActionsList
            // 
            this.TaskActionsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TaskActionsList.FormattingEnabled = true;
            this.TaskActionsList.Location = new System.Drawing.Point(3, 16);
            this.TaskActionsList.Name = "TaskActionsList";
            this.TaskActionsList.Size = new System.Drawing.Size(186, 301);
            this.TaskActionsList.TabIndex = 0;
            // 
            // TemplatesGroupBox
            // 
            this.TemplatesGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TemplatesGroupBox.Controls.Add(this.TemplatesList);
            this.TemplatesGroupBox.Location = new System.Drawing.Point(3, 329);
            this.TemplatesGroupBox.Name = "TemplatesGroupBox";
            this.TemplatesGroupBox.Size = new System.Drawing.Size(192, 311);
            this.TemplatesGroupBox.TabIndex = 1;
            this.TemplatesGroupBox.TabStop = false;
            this.TemplatesGroupBox.Text = "Templates";
            this.TemplatesGroupBox.Enter += new System.EventHandler(this.TemplatesGroupBox_Enter);
            // 
            // TemplatesList
            // 
            this.TemplatesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TemplatesList.FormattingEnabled = true;
            this.TemplatesList.Location = new System.Drawing.Point(3, 16);
            this.TemplatesList.Name = "TemplatesList";
            this.TemplatesList.Size = new System.Drawing.Size(186, 292);
            this.TemplatesList.TabIndex = 0;
            // 
            // DetailsPanel
            // 
            this.DetailsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.DetailsPanel.Location = new System.Drawing.Point(211, 0);
            this.DetailsPanel.Name = "DetailsPanel";
            this.DetailsPanel.Size = new System.Drawing.Size(793, 652);
            this.DetailsPanel.TabIndex = 2;
            this.DetailsPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DetailsPanel_Paint);
            // 
            // SelectorContainer
            // 
            this.SelectorContainer.Controls.Add(this.TaskActionGroupBox);
            this.SelectorContainer.Controls.Add(this.TemplatesGroupBox);
            this.SelectorContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.SelectorContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.SelectorContainer.Location = new System.Drawing.Point(0, 0);
            this.SelectorContainer.Name = "SelectorContainer";
            this.SelectorContainer.Size = new System.Drawing.Size(205, 652);
            this.SelectorContainer.TabIndex = 0;
            this.SelectorContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.SelectorContainer_Paint);
            // 
            // TaskPlansViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 652);
            this.Controls.Add(this.SelectorContainer);
            this.Controls.Add(this.DetailsPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskPlansViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TaskPlansViewer";
            this.Load += new System.EventHandler(this.TaskPlansViewer_Load);
            this.TaskActionGroupBox.ResumeLayout(false);
            this.TemplatesGroupBox.ResumeLayout(false);
            this.SelectorContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox TaskActionGroupBox;
        public System.Windows.Forms.GroupBox TemplatesGroupBox;
        public System.Windows.Forms.Panel DetailsPanel;
        public System.Windows.Forms.ListBox TaskActionsList;
        public System.Windows.Forms.ListBox TemplatesList;
        private System.Windows.Forms.FlowLayoutPanel SelectorContainer;
    }
}