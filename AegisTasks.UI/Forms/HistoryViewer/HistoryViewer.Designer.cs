namespace AegisTasks.UI.Forms
{
    partial class HistoryViewer
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
            this.HistoryViewerTable = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.HistoryViewerTable)).BeginInit();
            this.SuspendLayout();
            // 
            // HistoryViewerTable
            // 
            this.HistoryViewerTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.HistoryViewerTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HistoryViewerTable.Location = new System.Drawing.Point(0, 0);
            this.HistoryViewerTable.MultiSelect = false;
            this.HistoryViewerTable.Name = "HistoryViewerTable";
            this.HistoryViewerTable.ReadOnly = true;
            this.HistoryViewerTable.Size = new System.Drawing.Size(990, 551);
            this.HistoryViewerTable.TabIndex = 0;
            // 
            // HistoryViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 551);
            this.Controls.Add(this.HistoryViewerTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HistoryViewer";
            this.Text = "HistoryViewer";
            this.Load += new System.EventHandler(this.HistoryViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.HistoryViewerTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView HistoryViewerTable;
    }
}