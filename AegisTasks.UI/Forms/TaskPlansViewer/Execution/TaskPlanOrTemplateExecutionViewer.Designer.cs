namespace AegisTasks.UI.Forms
{
    partial class TaskPlanOrTemplateExecutionViewer
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
            this.ExecutionHistoryTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ExecutionHistoryTextBox
            // 
            this.ExecutionHistoryTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExecutionHistoryTextBox.Location = new System.Drawing.Point(0, 0);
            this.ExecutionHistoryTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ExecutionHistoryTextBox.Multiline = true;
            this.ExecutionHistoryTextBox.Name = "ExecutionHistoryTextBox";
            this.ExecutionHistoryTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ExecutionHistoryTextBox.Size = new System.Drawing.Size(1067, 554);
            this.ExecutionHistoryTextBox.TabIndex = 0;
            this.ExecutionHistoryTextBox.TextChanged += new System.EventHandler(this.ExecutionHistoryTextBox_TextChanged);
            // 
            // TaskPlanOrTemplateExecutionViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.ExecutionHistoryTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskPlanOrTemplateExecutionViewer";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.TaskPlanExecution_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox ExecutionHistoryTextBox;
    }
}