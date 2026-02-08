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
            panel1 = new Panel();
            TabBar.SuspendLayout();
            SuspendLayout();
            // 
            // TabBar
            // 
            TabBar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TabBar.BackColor = SystemColors.ControlDark;
            TabBar.Controls.Add(HideButton);
            TabBar.Location = new Point(0, 0);
            TabBar.Margin = new Padding(3, 4, 3, 4);
            TabBar.Name = "TabBar";
            TabBar.Size = new Size(596, 40);
            TabBar.TabIndex = 0;
            // 
            // HideButton
            // 
            HideButton.FlatStyle = FlatStyle.Flat;
            HideButton.Location = new Point(0, 0);
            HideButton.Margin = new Padding(3, 4, 3, 4);
            HideButton.Name = "HideButton";
            HideButton.Size = new Size(40, 40);
            HideButton.TabIndex = 0;
            HideButton.Text = "S";
            HideButton.UseVisualStyleBackColor = true;
            HideButton.Click += HideButton_Click;
            // 
            // ToolBar
            // 
            ToolBar.BackColor = SystemColors.AppWorkspace;
            ToolBar.Location = new Point(0, 40);
            ToolBar.Margin = new Padding(3, 4, 3, 4);
            ToolBar.Name = "ToolBar";
            ToolBar.Size = new Size(40, 358);
            ToolBar.TabIndex = 1;
            // 
            // ListPanel
            // 
            ListPanel.AccessibleRole = AccessibleRole.List;
            ListPanel.BackColor = Color.Gray;
            ListPanel.BorderStyle = BorderStyle.FixedSingle;
            ListPanel.Location = new Point(40, 40);
            ListPanel.Margin = new Padding(3, 4, 3, 4);
            ListPanel.Name = "ListPanel";
            ListPanel.Size = new Size(247, 358);
            ListPanel.TabIndex = 2;
            ListPanel.Click += ListPanel_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlLightLight;
            panel1.Location = new Point(451, 41);
            panel1.Name = "panel1";
            panel1.Size = new Size(296, 357);
            panel1.TabIndex = 1;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Wheat;
            ClientSize = new Size(1019, 492);
            Controls.Add(ListPanel);
            Controls.Add(panel1);
            Controls.Add(ToolBar);
            Controls.Add(TabBar);
            Margin = new Padding(3, 4, 3, 4);
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
        private Panel panel1;
    }
}