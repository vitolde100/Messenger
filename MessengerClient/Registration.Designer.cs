namespace MessengerClient
{
    partial class Registration
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            NameBox = new TextBox();
            IPBox = new TextBox();
            ConnectButton = new Button();
            NameLable = new Label();
            label1 = new Label();
            ConnectionExLable = new Label();
            SuspendLayout();
            // 
            // NameBox
            // 
            NameBox.BorderStyle = BorderStyle.FixedSingle;
            NameBox.Location = new Point(23, 28);
            NameBox.Name = "NameBox";
            NameBox.Size = new Size(165, 23);
            NameBox.TabIndex = 1;
            // 
            // IPBox
            // 
            IPBox.Location = new Point(23, 92);
            IPBox.Name = "IPBox";
            IPBox.Size = new Size(165, 23);
            IPBox.TabIndex = 2;
            // 
            // ConnectButton
            // 
            ConnectButton.Font = new Font("Segoe UI", 14F);
            ConnectButton.Location = new Point(23, 190);
            ConnectButton.Name = "ConnectButton";
            ConnectButton.Size = new Size(165, 40);
            ConnectButton.TabIndex = 0;
            ConnectButton.Text = "Connect";
            ConnectButton.UseVisualStyleBackColor = true;
            ConnectButton.Click += ConnectButton_Click;
            // 
            // NameLable
            // 
            NameLable.AutoSize = true;
            NameLable.Location = new Point(23, 12);
            NameLable.Name = "NameLable";
            NameLable.Size = new Size(39, 15);
            NameLable.TabIndex = 3;
            NameLable.Text = "Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 74);
            label1.Name = "label1";
            label1.Size = new Size(48, 15);
            label1.TabIndex = 4;
            label1.Text = "IP : Port";
            // 
            // ConnectionExLable
            // 
            ConnectionExLable.AutoSize = true;
            ConnectionExLable.ForeColor = Color.Red;
            ConnectionExLable.Location = new Point(23, 172);
            ConnectionExLable.Name = "ConnectionExLable";
            ConnectionExLable.Size = new Size(26, 15);
            ConnectionExLable.TabIndex = 5;
            ConnectionExLable.Text = "test";
            ConnectionExLable.TextAlign = ContentAlignment.MiddleCenter;
            ConnectionExLable.Visible = false;
            // 
            // Registration
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(215, 256);
            Controls.Add(ConnectionExLable);
            Controls.Add(label1);
            Controls.Add(NameLable);
            Controls.Add(IPBox);
            Controls.Add(NameBox);
            Controls.Add(ConnectButton);
            Name = "Registration";
            Text = "Registration";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox NameBox;
        private TextBox IPBox;
        private Button ConnectButton;
        private Label NameLable;
        private Label label1;
        private Label ConnectionExLable;
    }
}
