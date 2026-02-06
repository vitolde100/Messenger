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
            TabBar = new Panel();
            HideButton = new Button();
            ToolBar = new Panel();
            ListPanel = new Panel();
            TabBar.SuspendLayout();
            SuspendLayout();
            // 
            // TabBar
            // 
            TabBar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TabBar.BackColor = SystemColors.ControlDark;
            TabBar.Controls.Add(HideButton);
            TabBar.Location = new Point(0, 0);
            TabBar.Name = "TabBar";
            TabBar.Size = new Size(320, 30);
            TabBar.TabIndex = 0;
            // 
            // HideButton
            // 
            HideButton.FlatStyle = FlatStyle.Flat;
            HideButton.Location = new Point(0, 0);
            HideButton.Name = "HideButton";
            HideButton.Size = new Size(30, 30);
            HideButton.TabIndex = 0;
            HideButton.Text = "S";
            HideButton.UseVisualStyleBackColor = true;
            HideButton.Click += HideButton_Click;
            // 
            // ToolBar
            // 
            ToolBar.BackColor = SystemColors.AppWorkspace;
            ToolBar.Location = new Point(0, 30);
            ToolBar.Name = "ToolBar";
            ToolBar.Size = new Size(30, 269);
            ToolBar.TabIndex = 1;
            // 
            // ListPanel
            // 
            ListPanel.BackColor = Color.Gray;
            ListPanel.BorderStyle = BorderStyle.FixedSingle;
            ListPanel.Location = new Point(30, 30);
            ListPanel.Name = "ListPanel";
            ListPanel.Size = new Size(600, 269);
            ListPanel.TabIndex = 2;
            ListPanel.Click += ListPanel_Click;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.InfoText;
            ClientSize = new Size(1180, 676);
            Controls.Add(ListPanel);
            Controls.Add(ToolBar);
            Controls.Add(TabBar);
            Name = "ChatForm";
            Text = "Work window";
            Resize += Chat_Resize;
            TabBar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel TabBar;
        private Panel ToolBar;
        private Button HideButton;
        private Panel ListPanel;
    }
}