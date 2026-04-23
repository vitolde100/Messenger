namespace MessengerClient
{
    partial class ChatForm
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
            MainPanel = new Panel();
            SuspendLayout();
            // 
            // MainPanel
            // 
            MainPanel.BackColor = SystemColors.ButtonFace;
            MainPanel.Location = new Point(0, 0);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(200, 100);
            MainPanel.TabIndex = 0;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(878, 521);
            Controls.Add(MainPanel);
            ForeColor = SystemColors.ActiveCaption;
            Name = "ChatForm";
            Text = "Work window";
            Load += ChatForm_Load;
            SizeChanged += ChatForm_SizeChanged;
            ResumeLayout(false);
        }

        #endregion

        private Panel MainPanel;
    }
}